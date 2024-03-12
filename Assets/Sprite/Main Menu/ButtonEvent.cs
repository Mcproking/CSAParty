using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour 
{
    public void startGame(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }

    public void optionMenu()
    {

    }

    public void exitGame()
    {
        Application.Quit();
    }
}
