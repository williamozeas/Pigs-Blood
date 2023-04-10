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

    private Vector3 inspectPos = new Vector3(0f, 5.38f, -4.81f);
    private Quaternion inspectRot = Quaternion.Euler(-10f, 0f, 0f);

    private Vector3 lastPos;
    private Quaternion lastRot;

    bool wasTransitioning;

    protected virtual void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        wasTransitioning = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (grabbed)
        {
            float mouseY = Input.mousePosition.y;

            if (mouseY > 0.15f * Screen.height)
            {
                if (wasTransitioning){
                    transform.position = lastPos;
                    transform.rotation = lastRot;
                    wasTransitioning = false;
                }
                float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, mouseY, distanceToScreen ));
                transform.position = new Vector3( posMove.x + offset.x, transform.position.y, posMove.z  + offset.z);

                lastPos = transform.position;
                lastRot = transform.rotation;
            } else if (mouseY <= 0.15f * Screen.height && mouseY > 0.05f * Screen.height)
            {
                wasTransitioning = true;
                float newX = (Input.mousePosition.x - (Screen.width / 2f))/ Screen.width * 2.5f;
                Vector3 slerpWith = new Vector3(newX, lastPos.y, lastPos.z);
                transform.position = Vector3.Slerp(slerpWith, inspectPos, (.15f * Screen.height- mouseY) / (.1f * Screen.height));
                transform.rotation = Quaternion.Slerp(lastRot, inspectRot, (.15f * Screen.height- mouseY) / (.1f * Screen.height));
            } else
            {
                transform.position = inspectPos;
                transform.rotation = inspectRot;
                Inspect();
                grabbed = false;
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
    public virtual void Inspect()
    {
        Debug.Log("in");
        prevState = GameManager.Instance.PlayerState;
        GameManager.Instance.PlayerState = PlayerState.Inspecting;
    }
    
    //put item back (center of table?)
    public virtual void UnInspect()
    {
        GameManager.Instance.PlayerState = prevState;
    }
}
