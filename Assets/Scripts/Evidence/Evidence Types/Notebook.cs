using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Notebook : FlippableEvidence<NotebookPage>, IStatementHolder
{
    [SerializeField] private GameObject statementPrefab;
    [SerializeField] private int statementsPerPage = 5;
    // [SerializeField] private List<Transform> pages = new List<Transform>();
    //note: pages already exists in parent class
    
    private List<Statement> statements;
    private Canvas canvas;

    private Vector3 frontPos = new Vector3(0f, .006f, 0f);
    private Vector3 backPos = Vector3.zero;

    private Quaternion rightRot = Quaternion.identity;
    private Quaternion leftRot = Quaternion.Euler(0f, 0f, 180f);

    [SerializeField] GameObject pagePrefab;

    protected override void Awake()
    {
        base.Awake();
        type = DocType.Notebook;
        
        statements = GetComponentsInChildren<Statement>().ToList();
        for (int i = 0; i < 18; i++)
        {
            AddStatement(i.ToString(), "a");
        }
    }

    //TODO: clear statements on new interrogation
    protected void Start()
    {
        foreach (Statement statement in statements)
        {
            statement.holder = this;
        }
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public override void OnPresent()
    {
        Inspect();
        SetStatementsActive(true);
    }

    public override void UnInspect()
    {
        SetStatementsActive(false);
        base.UnInspect();
    }

    public void AddStatement(string ID, string display)
    {
        if (statements.Find(statement => statement.ID == ID))
        {
            return;
        }
        NotebookPage pageToAdd = pages.Last();
        if (pageToAdd.statements.Count >= statementsPerPage)
        {
            Debug.LogError("Not enough pages to hold all these statements!");
            //TODO: Add new page! & set pageToAdd
            NotebookPage newPage = Instantiate(pagePrefab, backPos, rightRot, transform).GetComponent<NotebookPage>();
            pages.Add(newPage);
            pageToAdd =  newPage;
        }
        Statement newStatement = Instantiate(statementPrefab, pageToAdd.pageColumn).GetComponent<Statement>();
        newStatement.ID = ID;
        newStatement.Display = display;
        newStatement.holder = this;
        statements.Add(newStatement);
        pageToAdd.statements.Add(newStatement);
    }
    
    private void SetStatementsActive(bool active)
    {
        foreach (Statement statement in statements)
        {
            statement.SetButtonEnabled(active);
        }
    }

    public void OnSelectStatement(Statement statement)
    {
        Debug.Log("Got statement with ID: " + statement.ID);
        UnInspect();
        GameManager.Instance.YarnCommandManager.RespondToEvidence(statement.ID);
    }

    public void ResetNotebook()
    {
        for (int i = statements.Count - 1; i >= 0; i--)
        {
            Destroy(statements[i].gameObject);
        }
        statements.Clear();
        foreach (var page in pages)
        {
            page.statements.Clear();
        }
    }

    public void RemoveStatement(string idToRemove)
    {
        statements.RemoveAll(statement => statement.ID == idToRemove);
        foreach (var page in pages)
        {
            page.statements.RemoveAll(statement => statement.ID == idToRemove);
        }
    }
    
    public override void Populate()
    {
        
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
        Transform prevPage = null;
        bool curIsZero = true;
        if (currentPage != 0)
        {
            curIsZero = false;
        }
        if (!curIsZero)
        {
            prevPage = pages[currentPage - 1].transform;
        }

        float timeElapsed = 0;
        float totalAngleRotated = 0;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseInOutQuad(0, 1, timeElapsed / time);

            float angleToRotate = 180f * percent - totalAngleRotated;
            
            flippingPage.Rotate(new Vector3(0f, 0f, angleToRotate));
            totalAngleRotated = 180f * percent;
            
            //Bring next page up and prev page down
            float upPercent = EasingFunction.EaseOutQuint(0, 1, timeElapsed / time);
            nextPage.localPosition = Vector3.Slerp(backPos, frontPos, upPercent);
            if (!curIsZero)
            {
                prevPage.localPosition = Vector3.Slerp(frontPos, backPos, upPercent);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        flippingPage.localRotation = leftRot;
        nextPage.localPosition = frontPos;
        if (!curIsZero)
        {
            prevPage.localPosition = backPos;
        }
    }

    private IEnumerator FlipLeftAnim(float time)
    {
        Transform flippingPage = pages[currentPage - 1].transform;
        Debug.Log(flippingPage.localRotation);
        Transform nextPage = pages[currentPage].transform;
        Transform prevPage = null;
        bool curIsOne = true;
        if (currentPage != 1)
        {
            curIsOne = false;
        }

        if (!curIsOne)
        {
            prevPage = pages[currentPage - 2].transform;
        }

        float timeElapsed = 0;
        float totalAngleRotated = 0;
        while (timeElapsed < time)
        {
            float percent = EasingFunction.EaseInOutQuad(0, 1, timeElapsed / time);

            float angleToRotate = 180f * percent - totalAngleRotated;

            flippingPage.Rotate(new Vector3(0f, 0f, -angleToRotate));
            totalAngleRotated = 180f * percent;
            
            //Bring next page up and prev page down
            float upPercent = EasingFunction.EaseOutQuint(1, 0, timeElapsed / time);
            nextPage.localPosition = Vector3.Slerp(backPos, frontPos, upPercent);
            if (!curIsOne)
            {
                prevPage.localPosition = Vector3.Slerp(frontPos, backPos, upPercent);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        flippingPage.localRotation = rightRot; 
        nextPage.localPosition = backPos;
        if (!curIsOne)
        {
            prevPage.localPosition = frontPos;
        }
    }
}
