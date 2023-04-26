using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatementHolder
{
    public void OnSelectStatement(Statement statement);
    public bool IsInspected();
}
