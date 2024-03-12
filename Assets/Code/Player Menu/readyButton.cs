using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class readyButton : MonoBehaviour, IPointerClickHandler
{
    private Button button;
    private Transform parentButton;
    private TMP_InputField NameInput;
    private Button[] skinSelectButton;
    private string lineUp;


    public masterPlayer_Selection selection;
    public CharacterManager characterManager;

    void Start()
    {
        button = GetComponent<Button>();
        parentButton = button.transform.parent.GetComponentInChildren<Transform>();
        NameInput = parentButton.Find("Name Area").Find("Name Input").GetComponent<TMP_InputField>();
        skinSelectButton = parentButton.Find("Buttons").GetComponentsInChildren<Button>();
        //print(NameInput.text);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            switch (parentButton.name)
            {
                case "Player 1":
                    lineUp = "P1";
                    break;
                case "Player 2":
                    lineUp = "P2";
                    break;
                case "Player 3":
                    lineUp = "P3";
                    break;
                case "Player 4":
                    lineUp = "P4";
                    break;
            }

            PlayerData playerData = new PlayerData(NameInput.text, characterManager.getSelectionOption(), true, lineUp);
            if(NameInput.text == "" || NameInput.text == null)
            {
                print("Cannot have empty name");
                return;
            }

            if (selection.checkPlayer(playerData))
            {
                print("remove");
                NameInput.interactable = !NameInput.interactable;
                selection.deletePlayer(selection.findPlayer(playerData));
                foreach(Button button in skinSelectButton) { button.interactable = !button.interactable; }
                selection.unready(parentButton.gameObject);
            }
            else
            {
                print("added");
                NameInput.interactable = !NameInput.interactable;
                foreach(Button button in skinSelectButton) { button.interactable = !button.interactable; }
                selection.addPlayer(playerData);
                selection.ready(parentButton.gameObject);
            }
            //print(characterManager.getSelectionOption());
            //print(PlayerName.text);
        }
    }
}
