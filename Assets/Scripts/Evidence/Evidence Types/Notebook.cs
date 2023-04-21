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

    private NotebookPage currentRightPage;
    private NotebookPage currentLeftPage;
    
    private List<Statement> statements;
    private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        type = DocType.Notebook;
        
        statements = GetComponentsInChildren<Statement>().ToList();
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
    
    public override void Populate()
    {
        
    }
}
