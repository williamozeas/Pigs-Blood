using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Statement : MonoBehaviour
{
    public string ID;
    public string Display
    {
        get => display;
        set => SetDisplayText(value);
    }
    [SerializeField] private string display;

    private Button button;
    [SerializeField] private TextMeshProUGUI label;
    public IStatementHolder holder;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.enabled = false;
        // button.onClick.AddListener(OnSelect);
        SetDisplayText(display);
    }

    public void OnSelect()
    {
        Debug.Log("aaa");
        holder.OnSelectStatement(this);
    }

    public void SetButtonEnabled(bool newEnabled)
    {
        button.enabled = newEnabled;
    }

    private void SetDisplayText(string text)
    {
        label.text = text;
        display = text;
    }

    
}
