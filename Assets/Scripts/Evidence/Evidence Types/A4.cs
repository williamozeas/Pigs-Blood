using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A4 : FlippableEvidence<A4Page>
{
    [SerializeField] private GameObject pagePrefab;

    private Vector3 frontPos = new Vector3(-.284f, .001f, .419f);
    private Vector3 backPos = new Vector3(-.284f, 0f, .419f);

    private Quaternion frontRot = Quaternion.identity;
    private Quaternion cornerBackRot = Quaternion.Euler(-180f, 0f, 0f);
    private Quaternion pageBackRot = Quaternion.Euler(-360f, 0f, 0f);

    [SerializeField] private Transform cornerT;

    [TextArea(3, 10)] public List<string> pagesText = new List<string>();
    
    protected override void Awake ()
    {
        type = DocType.A4;
        if (pagesText.Count == 0)
        {
            pagesText.Add(id); 
        }
        base.Awake();
    }
    
    public override void Populate()
    {
        base.Populate();
        // foreach (string pageText in pagesText)
        for(int i = 0; i < pagesText.Count; i++)
        {
            string pageText = pagesText[i];
            A4Page newPage = Instantiate(pagePrefab, transform).GetComponent<A4Page>();
            newPage.pageText = pageText;
            newPage.Populate();
            pages.Add(newPage);
            if (i == currentPage)
            {
                newPage.transform.localPosition = frontPos;
                newPage.transform.GetChild(0).localRotation = frontRot;
            }
            else
            {
                newPage.transform.localPosition = backPos;
                newPage.transform.GetChild(0).localRotation = pageBackRot;
            }
        }
    }

    public override void FlipRight()
    {
        StartCoroutine(FlipRightAnim(1f));
        base.FlipRight();
    }

    public override void FlipLeft()
    {
        StartCoroutine(FlipLeftAnim(1f));
        base.FlipLeft();
    }

    private IEnumerator FlipRightAnim(float time)
    {

        Transform flippingPage = pages[currentPage].transform;
        Transform nextPage = pages[currentPage + 1].transform;
        
        float timeElapsed = 0;
        float totalAngleRotated = 0;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseInOutQuad(0, 1, timeElapsed / time);

            Debug.Log(currentPage);

            float angleToRotate = -360f * percent - totalAngleRotated; 

            //Flip Corner
            if (currentPage == 1)
            {
                cornerT.GetChild(0).Rotate(new Vector3(angleToRotate / 2f, 0f, 0f));
            }
            
            //Flip front page
            // flippingPage.localPosition = Vector3.Slerp(frontPos, backPos, percent);
            // flippingPage.GetChild(0).localRotation = Quaternion.Euler(-360f * percent, 0f, 0f);
            
            flippingPage.Rotate(new Vector3(angleToRotate, 0f, 0f));
            flippingPage.localPosition = Vector3.Slerp(frontPos, backPos, percent);
            totalAngleRotated = -360f * percent;
            
            //Bring back page up
            float upPercent = EasingFunction.EaseOutQuint(0, 1, timeElapsed / time);
            nextPage.localPosition = Vector3.Slerp(backPos, frontPos, upPercent);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        cornerT.GetChild(0).localRotation = cornerBackRot;
        flippingPage.localPosition = backPos;
        // flippingPage.GetChild(0).localRotation = pageBackRot;

        nextPage.localPosition = frontPos;
    }

    private IEnumerator FlipLeftAnim(float time)
    {

        Transform flippingPage = pages[currentPage - 1].transform;
        Transform lastPage = pages[currentPage].transform;

        float timeElapsed = 0;
        float totalAngleRotated = 0;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseOutCubic(1, 0, timeElapsed / time);

            float angleToRotate = -360f * percent - totalAngleRotated;

            Debug.Log(angleToRotate);

            //Flip Corner
            if (currentPage == 0)
            {
                cornerT.GetChild(0).Rotate(new Vector3(angleToRotate / 2f, 0f, 0f));
            }
            
            //Flip front page
            // flippingPage.localPosition = Vector3.Slerp(frontPos, backPos, percent);
            // flippingPage.GetChild(0).localRotation = Quaternion.Euler(-360f * percent, 0f, 0f);
            flippingPage.Rotate(new Vector3(angleToRotate, 0f, 0f));
            flippingPage.localPosition = Vector3.Slerp(frontPos, backPos, percent);
            totalAngleRotated = -360f * percent;

            //Bring back page down
            float downPercent = EasingFunction.EaseInQuint(1, 0, timeElapsed / time);
            lastPage.localPosition = Vector3.Slerp(backPos, frontPos, downPercent);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (currentPage == 0)
        {
            cornerT.GetChild(0).localRotation = frontRot;
        } else
        {
            cornerT.GetChild(0).localRotation = cornerBackRot;
        }
        flippingPage.localPosition = frontPos;
        // flippingPage.GetChild(0).localRotation = frontRot;

        lastPage.localPosition = backPos;
    }
}
