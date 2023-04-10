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

    protected virtual void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (grabbed)
        {
            float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen ));
            transform.position = new Vector3( posMove.x + offset.x, transform.position.y, posMove.z  + offset.z);
        }
    }
    
    public virtual void Dropped()
    {
        rb.useGravity = true;
        grabbed = false;
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
        prevState = GameManager.Instance.PlayerState;
        GameManager.Instance.PlayerState = PlayerState.Inspecting;
    }
    
    //put item back (center of table?)
    public virtual void UnInspect()
    {
        GameManager.Instance.PlayerState = prevState;
    }
}
