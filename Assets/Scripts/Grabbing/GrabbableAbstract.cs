using UnityEngine;

public enum DocType{A4}

public abstract class GrabbableAbstract : MonoBehaviour
{
    protected DocType type;

    protected Rigidbody rb;

    protected bool grabbed;

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
            transform.position = new Vector3( posMove.x, transform.position.y, posMove.z );
        }
    }
    
    public void Dropped()
    {
        rb.useGravity = true;
        grabbed = false;
    }

    public void Grabbed(GameObject grabber, Vector3 offset)
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.position += new Vector3(0f, .3f, 0f);
        rb.useGravity = false;
        grabbed = true;
    }
}
