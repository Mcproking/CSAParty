using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Load_Outfit : MonoBehaviour
{
    [Header("Player Outfit")]
    public CharacterSelection characterDB;

    public SpriteRenderer characterRenderer;

    public PlayersData playersData;

    private GameObject GO_PlayerSelectionMaster;
    private masterPlayer_Selection masterPlayerSelection;

    void Start()
    {
        GO_PlayerSelectionMaster = GameObject.Find("master PlayerSkin");
        masterPlayerSelection = GO_PlayerSelectionMaster.GetComponent<masterPlayer_Selection>();



        if (this.name == "Player 1")
        {
            PlayerData playerName = masterPlayerSelection.getPlayerData("P1");
            GetComponent<Player2D>().Name = playerName.Name;
            UpdateCharactor(playerName.SkinSelection);
        }

        if (this.name == "Player 2")
        {
            PlayerData playerName = masterPlayerSelection.getPlayerData("P2");
            GetComponent<Player2D>().Name = playerName.Name;
            UpdateCharactor(playerName.SkinSelection);
        }
        if (this.name == "Player 3")
        {
            PlayerData playerName = masterPlayerSelection.getPlayerData("P3");
            GetComponent<Player2D>().Name = playerName.Name;
            UpdateCharactor(playerName.SkinSelection);
        }
        if (this.name == "Player 4")
        {
            PlayerData playerName = masterPlayerSelection.getPlayerData("P4");
            GetComponent<Player2D>().Name = playerName.Name;
            UpdateCharactor(playerName.SkinSelection);
        }
    }

    public void UpdateCharactor(int selectedOption)
    {
        Character character = characterDB.GetCharater(selectedOption);
        characterRenderer.sprite = character.characterSprite;
    }

//    private void Load()
//    {
//        selectionOption = PlayerPrefs.GetInt("SelectedOption");
//    }
}