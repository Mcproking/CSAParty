using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class returnButtonText : MonoBehaviour, IPointerClickHandler
{
    private Button button;
    private string ReturnedText;

    void Start() { 
        button = GetComponent<Button>(); 
    }

    void doReturnText(Button button)
    {
        ReturnedText = GetComponentInChildren<TextMeshProUGUI>().text;
    }

    public string getReturnedText()
    {
        return ReturnedText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            doReturnText(button);
        }
    }
}
