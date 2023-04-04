using UnityEngine;

public enum DocType{A4}

public interface IGrabbable
{
    DocType Type {get;}
    void Grabbed(GameObject grabber, Vector3 offset);
    void Dropped();
}
