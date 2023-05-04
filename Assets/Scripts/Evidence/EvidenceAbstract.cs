using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum DocType{ A4, Notebook }

public enum PageDirection {
    Left, Right
}

public abstract class EvidenceAbstract : MonoBehaviour
{
    public string id;

    public DocType Type => type;
    //used to prevent notebook from being clicked out of on present
    [HideInInspector] public bool canBeClickedOff = true; 
    protected DocType type;

    protected Rigidbody rb;

    protected bool grabbed;
    protected bool inspected;

    private PlayerState prevState;

    private Vector3 offset;

    [SerializeField] protected Vector3 inspectPos = new Vector3(0f, 5.33f, -4.75f);
    [SerializeField] protected Quaternion inspectRot = Quaternion.Euler(-10f, 0f, 0f);

    private Vector3 lastPos;
    private Quaternion lastRot;

    private Camera mainCam;

    protected virtual void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
        Populate();
    }
    
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (transform.position.y < -2f)
        {
            transform.position = EvidenceManager.Instance.GetResetPoint();
        }
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
                float distanceToScreen = CameraManager.Instance.mainCam.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 posMove = CameraManager.Instance.mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, mouseY, distanceToScreen ));
                transform.position = new Vector3( posMove.x + offset.x, transform.position.y, posMove.z  + offset.z);

                lastPos = transform.position;
                lastRot = transform.rotation;
            } else if (mouseY <= 0.15f * Screen.height && mouseY > 0.09f * Screen.height)
            { //left in so that you can partially transition in cuz that's cool but will run automatically on Inspect()
                float newX = (Input.mousePosition.x - (Screen.width / 2f))/ Screen.width * 2.5f;
                Vector3 slerpWith = new Vector3(newX, lastPos.y, lastPos.z);
                float percent = EasingFunction.EaseOutQuad(0, 1, (.15f * Screen.height- mouseY) / (.1f * Screen.height));
                transform.position = Vector3.Slerp(slerpWith, inspectPos, percent);
                transform.rotation = Quaternion.Slerp(lastRot, inspectRot, percent);
                CameraManager.Instance.SetPaniniProjectionTransitionInspect(percent);
            } else
            {
                Inspect(0f);
            }
        }
    }
    
    //Set up UI from current values of variables
    public virtual void Populate()
    {
        
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
        float x = Random.Range(-2f, 2f);
        float z = Random.Range(-3.8f, -5f);
        Vector3 position = new Vector3(x, 3.6f, z);
        StartCoroutine(MoveToPos(position, 1.5f));
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
        inspected = true;

        prevState = GameManager.Instance.PlayerState;
        GameManager.Instance.PlayerState = PlayerState.Inspecting;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private IEnumerator InspectAnim(float time)
    {
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
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
        rb.velocity = Vector3.zero;
    }
    
    //put item back (center of table?)
    public virtual void UnInspect()
    {
        Dropped();
        inspected = false;
        canBeClickedOff = true;
        GameManager.Instance.PlayerState = prevState;
        float randTorque = Random.Range(-1f, 1f);
        rb.AddTorque(Vector3.up * randTorque, ForceMode.VelocityChange);
        rb.constraints = RigidbodyConstraints.None;
    }

    public bool IsInspected()
    {
        return inspected;
    }

    private IEnumerator MoveToPos(Vector3 pos, float time)
    {
        rb.useGravity = false;
        float timeElapsed = 0;
        Vector3 start = transform.position;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseInOutCubic(0, 1, timeElapsed / time);

            transform.position = Vector3.Lerp(start, pos, percent);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rb.useGravity = true;
    }
}
