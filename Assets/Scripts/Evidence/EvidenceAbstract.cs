using System.Collections;
using UnityEngine;

public enum DocType{ A4, Notebook }

public abstract class EvidenceAbstract : MonoBehaviour
{
    public string id;

    public DocType Type => type;
    protected DocType type;

    protected Rigidbody rb;

    protected bool grabbed;

    private PlayerState prevState;

    private Vector3 offset;

    private Vector3 inspectPos = new Vector3(0f, 5.33f, -4.75f);
    private Quaternion inspectRot = Quaternion.Euler(-10f, 0f, 0f);

    private Vector3 lastPos;
    private Quaternion lastRot;

    bool wasTransitioning;
    private Camera mainCam;

    public int totalPages;
    public int currentPage;

    protected virtual void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        wasTransitioning = false;
    }
    
    void Start()
    {
        mainCam = Camera.main;
        currentPage = 1;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (grabbed)
        {
            float mouseY = Input.mousePosition.y;
            
            if((GameManager.Instance.PlayerState == PlayerState.Review || GameManager.Instance.PlayerState == PlayerState.ChooseEvidence)
               && Input.GetMouseButtonUp(0))
            {
                Dropped();
            }

            if (mouseY > 0.15f * Screen.height)
            {
                if (wasTransitioning){
                    transform.position = lastPos;
                    transform.rotation = lastRot;
                    wasTransitioning = false;
                }
                float distanceToScreen = CameraManager.Instance.mainCam.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 posMove = CameraManager.Instance.mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, mouseY, distanceToScreen ));
                transform.position = new Vector3( posMove.x + offset.x, transform.position.y, posMove.z  + offset.z);

                lastPos = transform.position;
                lastRot = transform.rotation;
            } else if (mouseY <= 0.15f * Screen.height && mouseY > 0.05f * Screen.height)
            { //left in so that you can partially transition in cuz that's cool but will run automatically on Inspect()
                wasTransitioning = true;
                float newX = (Input.mousePosition.x - (Screen.width / 2f))/ Screen.width * 2.5f;
                Vector3 slerpWith = new Vector3(newX, lastPos.y, lastPos.z);
                float percent = EasingFunction.EaseOutQuad(0, 1, (.15f * Screen.height- mouseY) / (.1f * Screen.height));
                transform.position = Vector3.Slerp(slerpWith, inspectPos, percent);
                transform.rotation = Quaternion.Slerp(lastRot, inspectRot, percent);
            } else
            {
                Inspect(0f);
            }
        }
    }
    
    public virtual void Dropped()
    {
        rb.useGravity = true;
        grabbed = false;
        lastRot = Quaternion.identity;
    }

    public virtual void Grabbed(GameObject grabber, Vector3 newOffset)
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.position += new Vector3(0f, .3f, 0f);
        rb.useGravity = false;
        grabbed = true;
        offset = newOffset;
    }

    //put this here so that we can do things like make the player select a specific statement in the notebook etc.
    public virtual void OnPresent()
    {
        GameManager.Instance.YarnCommandManager.RespondToEvidence(id);
    }

    //bring item up for closer inspection
    public virtual void Inspect(float time = 1f)
    {
        grabbed = false;
        EvidenceManager.Instance.SetInspectedObject(this);
        lastPos = transform.position;
        lastRot = transform.rotation;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(InspectAnim(time));

        prevState = GameManager.Instance.PlayerState;
        GameManager.Instance.PlayerState = PlayerState.Inspecting;
    }

    private IEnumerator InspectAnim(float time)
    {
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            wasTransitioning = true;
            float newX = (Input.mousePosition.x - (Screen.width / 2f))/ Screen.width * 2.5f;
            Vector3 slerpWith = new Vector3(newX, lastPos.y, lastPos.z);
            float percent = EasingFunction.EaseOutCubic(0, 1, timeElapsed / time);
            transform.position = Vector3.Slerp(slerpWith, inspectPos, percent);
            transform.rotation = Quaternion.Slerp(lastRot, inspectRot, percent);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = inspectPos;
        transform.rotation = inspectRot;
    }
    
    //put item back (center of table?)
    public virtual void UnInspect()
    {
        Dropped();
        GameManager.Instance.PlayerState = prevState;
    }

    public virtual void TryFlip(bool flippingRight)
    {
        if (flippingRight && currentPage < totalPages)
        {
            FlipRight();
        } else if (!flippingRight && currentPage > 0)
        {
            FlipLeft();
        }
    }

    public virtual void FlipRight()
    {
        
    }

    public virtual void FlipLeft()
    {
        
    }
}
