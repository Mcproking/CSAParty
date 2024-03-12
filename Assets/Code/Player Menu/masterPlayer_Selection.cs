using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class masterPlayer_Selection : MonoBehaviour
{
    //player choose skin and name, player ready, change bgc and lock name and arrow, player unready, unlock name and arrow, wait update, lock name and arrow, check 4 player is ready, if 4 player is ready start game
    private PlayersData playersData;
    [SerializeField] private Color readyColor;
    [SerializeField] private Color unreadyColor;

    // player db related
    private void Awake()
    {
        DontDestroyOnLoad(this);
        playersData = new PlayersData();
    }

    public void addPlayer(PlayerData playerData)
    {
        playersData.playerDatas.Add(playerData);
    }

    public bool checkPlayer(PlayerData playerData)
    {
        foreach(PlayerData player in playersData.playerDatas)
        {
            if(playerData.lineUp == player.lineUp && playerData.SkinSelection == player.SkinSelection)
            {
                return true;
            }
        }
        return false;
    }

    public PlayerData findPlayer(PlayerData playerData)
    {
        foreach (PlayerData player in playersData.playerDatas)
        {
            if (playerData.Name == player.Name && playerData.SkinSelection == player.SkinSelection)
            {
                return player;
            }
        }
        return null;

    }

    public void deletePlayer(PlayerData playerData)
    {
        playersData.playerDatas.Remove(playerData);
    }

    public int lengthPlayer()
    {
        return playersData.playerDatas.Count;
    }

    public PlayerData getPlayerData(string lineUp)
    {
        foreach(PlayerData player in playersData.playerDatas )
        {
            if (player.lineUp == lineUp)
            {
                return player;
            }
            
        }
        return null;
    }


    // 
    public void ready(GameObject playerParent)
    {
        
        playerParent.GetComponent<Image>().color = readyColor;
    }

    public void unready(GameObject playerParent)
    {
        playerParent.GetComponent<Image>().color = unreadyColor;
    }

    public void LoadGame(int SceneID)
    {
        if(lengthPlayer() == 4)
        {
            foreach(PlayerData player in playersData.playerDatas)
            {
                if (!player.getReady())
                {
                    print("error, not all 4 player ready");
                    break;
                }
            }
            SceneManager.LoadScene(SceneID);
        }
        else
        {
            print("error, more/less than 4 player");
        }

    }

    public void printtemp()
    {
        print("print from masterplayer seleciotn");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


}
