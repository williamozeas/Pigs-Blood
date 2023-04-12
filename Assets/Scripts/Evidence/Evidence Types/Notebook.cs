using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Notebook : EvidenceAbstract, IStatementHolder
{
    [SerializeField] private GameObject statementPrefab;
    [SerializeField] private int statementsPerPage = 5;
    [SerializeField] private List<Transform> pages = new List<Transform>();
    
    private List<Statement> statements;
    private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        type = DocType.Notebook;
        
    }

    //TODO: clear statements on new interrogation
    protected void Start()
    {
        statements = GetComponentsInChildren<Statement>().ToList();
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
        int pageIndex = statements.Count % statementsPerPage;
        if (pageIndex >= pages.Count)
        {
            Debug.LogError("Not enough pages to hold all these statements!");
        }
        Statement newStatement = Instantiate(statementPrefab, pages[pageIndex]).GetComponent<Statement>();
        newStatement.ID = ID;
        newStatement.Display = display;
        newStatement.holder = this;
        statements.Add(newStatement);
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
        Debug.Log("Got statement with ID: " + id);
        GameManager.Instance.YarnCommandManager.RespondToEvidence(statement.ID);
    }

    public void ResetNotebook()
    {
        for (int i = statements.Count - 1; i > 0; i--)
        {
            Destroy(statements[i]);
        }
    }
    
    public override void Populate()
    {
        
    }
}
