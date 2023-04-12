using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A4 : EvidenceAbstract
{

    public GameObject br_corner;
    public GameObject bl_corner;

    private Vector3 frontPos = new Vector3(-.284f, .001f, .419f);
    private Vector3 backPos = new Vector3(-.284f, 0f, .419f);

    private Quaternion frontRot = Quaternion.identity;
    private Quaternion cornerBackRot = Quaternion.Euler(-180f, 0f, 0f);
    private Quaternion pageBackRot = Quaternion.Euler(-360f, 0f, 0f);

    [TextArea(3, 10)] public List<string> pages;
    
    protected override void Awake ()
    {
        base.Awake();
        type = DocType.A4;
    }
    
    public override void Inspect (float time = 1f)
    {
        base.Inspect(time);
        br_corner.SetActive(true);
    }

    public override void UnInspect()
    {
        base.UnInspect();
        br_corner.SetActive(false);
        bl_corner.SetActive(false);
    }

    public override void FlipRight()
    {
        StartCoroutine(FlipRightAnim(1f));
        currentPage++;
        bl_corner.SetActive(true);
        if (currentPage == totalPages) {
             br_corner.SetActive(false);
        }
    }

    public override void FlipLeft()
    {
        StartCoroutine(FlipLeftAnim(1f));
        currentPage--;
        br_corner.SetActive(true);
        if (currentPage == 1) {
             bl_corner.SetActive(false);
        }
    }

    private IEnumerator FlipRightAnim(float time)
    {

        Transform cornerT = transform.GetChild(0);

        Transform flippingPage = transform.GetChild(currentPage);
        Transform nextPage = transform.GetChild(currentPage + 1);

        

        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseInOutQuad(0, 1, timeElapsed / time);

            //Flip Corner
            if (currentPage == 1)
            {
                cornerT.GetChild(0).localRotation = Quaternion.Euler(-180f * percent, 0f, 0f);
            }
            
            //Flip front page
            flippingPage.localPosition = Vector3.Slerp(frontPos, backPos, percent);
            flippingPage.GetChild(0).localRotation = Quaternion.Euler(-360f * percent, 0f, 0f);

            //Bring back page up
            nextPage.localPosition = Vector3.Slerp(backPos, frontPos, percent);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (currentPage == 1)
        {
            cornerT.GetChild(0).localRotation = cornerBackRot;
        }
        flippingPage.localPosition = backPos;
        flippingPage.GetChild(0).localRotation = pageBackRot;

        nextPage.localPosition = frontPos;
    }

    private IEnumerator FlipLeftAnim(float time)
    {

        Transform cornerT = transform.GetChild(0);

        Transform flippingPage = transform.GetChild(currentPage - 1);
        Transform lastPage = transform.GetChild(currentPage);

        

        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseOutCubic(1, 0, timeElapsed / time);

            //Flip Corner
            if (currentPage == 2)
            {
                cornerT.GetChild(0).localRotation = Quaternion.Euler(-180f * percent, 0f, 0f);
            }
            
            //Flip front page
            flippingPage.localPosition = Vector3.Slerp(frontPos, backPos, percent);
            flippingPage.GetChild(0).localRotation = Quaternion.Euler(-360f * percent, 0f, 0f);

            //Bring back page down
            lastPage.localPosition = Vector3.Slerp(backPos, frontPos, percent);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (currentPage == 1)
        {
            cornerT.GetChild(0).localRotation = frontRot;
        }
        flippingPage.localPosition = frontPos;
        flippingPage.GetChild(0).localRotation = frontRot;

        lastPage.localPosition = backPos;
    }

    public override void Populate()
    {
        
    }
}
