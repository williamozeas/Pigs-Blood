using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Statement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        label.color = Color.black;
    }

    private void SetDisplayText(string text)
    {
        label.text = text;
        display = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.enabled)
        {
            label.color = new Color((float)232/255, (float)70/255, (float)21/255);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.enabled)
        {
            label.color = Color.black;
        }
    }
    
}
