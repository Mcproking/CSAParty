using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{

    [Header("Button Reference")]
    public button_interaction button_Interaction;


    [Header("Charater")]
    public CharacterSelection characterDB;

    public TextMeshProUGUI characterName;
    public SpriteRenderer characterRenderer;
    public TextMeshProUGUI charactetrDescription;
    public TextMeshProUGUI copyright;

    private int selectionOption = 0;

    // Start is called before the first frame update
    void Start()
    {
    
        selectionOption = 0;  
        copyright.text = "";

        UpdateCharactor(selectionOption);
    }

    public void NextOption()
    {
        selectionOption++; 

        if(selectionOption >= characterDB.CharacterCount)
        {
            selectionOption = 0;
        }

        UpdateCharactor(selectionOption);
        button_Interaction.ReenableButton(1);
    }

    public void BackOption()
    {
        selectionOption--;

        if(selectionOption < 0)
        {
            selectionOption = characterDB.CharacterCount - 1;
        }

        UpdateCharactor(selectionOption);
        button_Interaction.ReenableButton(0);
    }

    public void UpdateCharactor(int selectedOption)
    {
        Character character = characterDB.GetCharater(selectedOption);

        characterRenderer.sprite = character.characterSprite;
        characterName.text = character.characterName;
        charactetrDescription.text = character.description;
        if (character.copyright != "")
        {
            copyright.text = "\u00A9" + character.copyright;
        }
        else
        {
            copyright.text = "";
        }
    }

    public int getSelectionOption()
    {
        return selectionOption;
    }
}
