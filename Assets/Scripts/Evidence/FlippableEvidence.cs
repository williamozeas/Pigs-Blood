using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that allows flipping pages - requires EvidenceCorner corners to flip back and forth,
//and requires another class for each page to derive from GenericPage
public abstract class FlippableEvidence<T> : EvidenceAbstract, IFlippable where T : PageAbstract
{
    public int currentPage = 0;
    
    public List<T> pages = new List<T>();
    
    public EvidenceCorner br_corner;
    public EvidenceCorner bl_corner;

    public virtual void TryFlip(PageDirection direction)
    {
        if (direction == PageDirection.Right && currentPage < pages.Count - 1)
        {
            FlipRight();
        } 
        else if (direction == PageDirection.Left && currentPage >= 0)
        {
            FlipLeft();
        }
    }

    public override void Populate()
    {
        foreach (T page in pages)
        {
            if(page != null)
                Destroy(page.gameObject);
        }
        pages.Clear();
    }
    
    public override void Inspect (float time = 1f)
    {
        base.Inspect(time);
        br_corner.gameObject.SetActive(true);
    }
    
    public override void UnInspect()
    {
        base.UnInspect();
        br_corner.gameObject.SetActive(false);
        bl_corner.gameObject.SetActive(false);
    }

    public virtual void FlipRight()
    {
        currentPage++;
        bl_corner.gameObject.SetActive(true);
        if (currentPage == pages.Count - 1) {
             br_corner.gameObject.SetActive(false);
        }
    }

    public virtual void FlipLeft()
    {
        currentPage--;
        br_corner.gameObject.SetActive(true);
        if (currentPage == 0) {
            bl_corner.gameObject.SetActive(false);
        }
    }
}
