using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

public class master2D : MonoBehaviour
{
    public tiles2D tiles;
    public Gameloop2D gameloop;
    public ScoreManager scoreManager;


    // game loop 
    private bool playerStart;
    private bool gameEnd;
    private int playerTurn = 1;
    private IEnumerator enumerator;
    private bool loadedScene;

    [Header("Players Setting")]
    // Player 1
    private GameObject P1;
    private string P1_Tile;
    //Player 2
    private GameObject P2;
    private string P2_Tile;
    //Player 3
    private GameObject P3;
    private string P3_Tile;
    //Player 4
    private GameObject P4;
    private string P4_Tile;

    [Header("Move Setting")]
    public float cameraMoveSpeed = 1f;
    public float moveSpeed = 20;

    [Header("Game Setting")]
    public int pointConvertRatio = 5;
    public int pointAchive = 5;
    public int pointMultipler = 4;
    public int MaxRound = 5;
    private int CurrentRound = 1;
    public int questionTime = 15;
    public TextMeshProUGUI playerRoundText;

    int loopPlayer = 1;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //print("master start");

        //print("Start p1");
        //GameObject.Find("Player 1").GetComponent<Player2D>().manualAwake();
        //print("Start p2");
        //GameObject.Find("Player 2").GetComponent<Player2D>().manualAwake();
        //print("Start p3");
        //GameObject.Find("Player 3").GetComponent<Player2D>().manualAwake();
        //print("Start p4");
        //GameObject.Find("Player 4").GetComponent<Player2D>().manualAwake();


        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(x => x.name).ToArray();

        foreach(GameObject player in players)
        {
            if (player.name == "Player 1")
            {
                P1 = player;
                P1_Tile = tiles.getSpawnTile(player.GetComponent<Player2D>(), 1);
                var player_Component = player.GetComponent<Player2D>();
                scoreManager.AddScore(new Score(player_Component.Name, player_Component.getBigPoint(), player_Component.getSmallPoint()));
            }
            if (player.name == "Player 2")
            {
                P2 = player;
                P2_Tile = tiles.getSpawnTile(player.GetComponent<Player2D>(), 2);
                var player_Component = player.GetComponent<Player2D>();
                scoreManager.AddScore(new Score(player_Component.Name, player_Component.getBigPoint(), player_Component.getSmallPoint()));
            }
            if (player.name == "Player 3")
            {
                P3 = player;
                P3_Tile = tiles.getSpawnTile(player.GetComponent<Player2D>(), 3);
                var player_Component = player.GetComponent<Player2D>();
                scoreManager.AddScore(new Score(player_Component.Name, player_Component.getBigPoint(), player_Component.getSmallPoint()));

            }
            if (player.name == "Player 4")
            {
                P4 = player;
                P4_Tile = tiles.getSpawnTile(player.GetComponent<Player2D>(), 4);
                var player_Component = player.GetComponent<Player2D>();
                scoreManager.AddScore(new Score(player_Component.Name, player_Component.getBigPoint(), player_Component.getSmallPoint()));

            }
        }

        //print(P1_Tile);
        //print(P2_Tile);
        //print(P3_Tile);
        //print(P4_Tile);
    }

    private void FixedUpdate()
    {
        if (!playerStart)
        {
            //print("player round " + loopPlayer);
            playerRoundText.text = "Player " + loopPlayer;
            Player2D currentPlayer = null;
            string playerTile = null;

            //print(playerTurn);
            playerStart = true;


            switch (playerTurn)
            {
                case 1:
                    currentPlayer = P1.GetComponent<Player2D>();
                    playerTile = P1_Tile;
                    break;
                case 2:
                    currentPlayer = P2.GetComponent<Player2D>();
                    playerTile = P2_Tile;
                    break;
                case 3:
                    currentPlayer = P3.GetComponent<Player2D>();
                    playerTile = P3_Tile;
                    break;
                case 4:
                    currentPlayer = P4.GetComponent<Player2D>();
                    playerTile = P4_Tile;
                    break;
            }

            enumerator = gameloop.currentPlayerMove(currentPlayer, playerTile);
            //print("added loop player and start playermove");
            StartCoroutine(enumerator);
            // dont add anything below here
        }

        if (playerStart && gameEnd)
        { 
            if (!loadedScene)
            {
                loadedScene = true;
                SceneManager.LoadScene(3);
            }
        }
    }

    






    public int getPlayerTurn()
    {
        return playerTurn;
    }

    public void setPlayerTurn(int number)
    {
        playerTurn = number;
    }

    public void setPlayerStart(bool statment)
    {
        playerStart = statment;
    }

    public bool getPlayerStart()
    {
        return playerStart;
    }

    public int getLoopPlayer()
    {
        return loopPlayer;
    }

    public void resetLoopPlayer()
    {
        loopPlayer = 1;
    }

    public void addLoopPlayer()
    {
        loopPlayer++;
    }
    public void addRound()
    {
        if(CurrentRound == MaxRound)
        {
            // the thing will pop up and end the coroutine from the startcorotine
            StopCoroutine(enumerator);
            gameEnd = true;
            playerStart = true;
            print("game end");
        }
        CurrentRound += 1;

    }

    public int getCurrentRound()
    {
        return CurrentRound;
    }
}
