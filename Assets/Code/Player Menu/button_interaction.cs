using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button_interaction : MonoBehaviour
{
    private Button button;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// 0 = Left Arrow
    /// 1 = Right Arrow
    /// </summary>
    /// <param name="direction"></param>
    public void ReenableButton(int direction)
    {
        switch (direction)
        {
            case 0:
                button = GameObject.Find("Left Button").GetComponent<Button>();
                image = button.GetComponent<Image>();
                break;
            case 1:
                button = GameObject.Find("Right Button").GetComponent<Button>();
                image = button.GetComponent<Image>();
                break;
            default:
                Debug.Log("Reenable Button got issue");
                break;
        }

        if (!button.interactable)
        {
            button.interactable = true;
            image.sprite = button.spriteState.highlightedSprite;
        }
    }

}
