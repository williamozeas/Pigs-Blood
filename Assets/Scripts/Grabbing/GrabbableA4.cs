using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableA4 : MonoBehaviour, IGrabbable
{
    Rigidbody rb;

    bool grabbed;

    Vector3 grabOffset;

    public DocType Type => DocType.A4; 

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
        grabOffset = offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen ));
            transform.position = new Vector3( posMove.x, transform.position.y, posMove.z );
        }
    }
}
