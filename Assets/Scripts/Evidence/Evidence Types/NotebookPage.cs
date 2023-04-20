using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookPage : PageAbstract
{
    public Transform pageColumn;
    public List<Statement> statements;

    public override void Populate()
    {
        //don't populate anything bc will be blank on spawn
    }

    public override void Deactivate()
    {
        //TODO
    }
    
    public void AddStatement(Statement statement)
    {
        
    }
}
