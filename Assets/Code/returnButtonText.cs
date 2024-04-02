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
        print("do return trext is " +  ReturnedText);
    }

    public string getReturnedText()
    {
        print("send return text" + ReturnedText);
        return ReturnedText;
    }

    public void resetReturnText()
    {
        ReturnedText = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            doReturnText(button);
        }
    }
}
