using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class master : MonoBehaviour
{
    public dice Dice;
    public tiles Tiles;

    [Header("GUI Refernce")]
    public TextMeshProUGUI turn;
    public TextMeshProUGUI textDiceRoll;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI countdownTimer;
    public TextMeshProUGUI pointSmall;
    public TextMeshProUGUI pointBig;
    public Button buttonConvert;
    public Button buttonCancel;

    // Game Loop
    [Header("GameLoop Setting")]
    public float CountdownWaitingPlayerInput = 5.0f;
    private bool startTurn;
    private int diceResult;
    private GameObject[] players;
    private GameObject P1;
    private GameObject P2;
    private GameObject P3;
    private GameObject P4;

    // Dice
    [Header("Dice Setting")]
    public int minThrow;
    public int maxThrow;
    private int diceNumber;

    //Player
    [Header("Player Setting")]
    public float moveSpeed = 10;
    private int playersTurn = 1;

    // Start is called before the first frame update
    void Start()
    {
        countdownTimer.gameObject.SetActive(false);
        countdownTimer.text = "Timer\n" + CountdownWaitingPlayerInput.ToString();
        buttonCancel.gameObject.SetActive(false);
        buttonConvert.gameObject.SetActive(false);

        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.name == "Player 1")
            {
                Tiles.spawnTile(player.GetComponent<Player>(), 1);
                P1 = player;
            }
            if (player.name == "Player 2")
            {
                Tiles.spawnTile(player.GetComponent<Player>(), 2);
                P2 = player;
            }
            if (player.name == "Player 3")
            {
                Tiles.spawnTile(player.GetComponent<Player>(), 3);
                P3 = player;
            }
            if (player.name == "Player 4")
            {
                Tiles.spawnTile(player.GetComponent<Player>(), 4);
                P4 = player;
            }
        }


        Debug.Log(Tiles.p1Spawn);
        Debug.Log(Tiles.p2Spawn);
        Debug.Log(Tiles.p3Spawn);
        Debug.Log(Tiles.p4Spawn);
    }


    // player turn, get dice amount, calulate how many turn the player have left, 


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!startTurn)
        {
            startTurn = true;

            Player player = null;

            switch (playersTurn)
            {
                case 1:
                    turn.text = "Player 1";
                    player = P1.GetComponent<Player>();
                    break;
                case 2:
                    turn.text = "Player 2";
                    player = P2.GetComponent<Player>();
                    break;
                case 3:
                    turn.text = "Player 3";
                    player = P3.GetComponent<Player>();
                    break;
                case 4:
                    turn.text = "Player 4";
                    player = P4.GetComponent<Player>();
                    break;
            }
            if (player == null)
            {
                Debug.Log("Player assign in fixed update is null");
            }

            updatePoint(player);

            StartCoroutine(playerStart(player));


            // if i want to set anything more than player movement, i have to remove the bool start turn at player start function
        }
    }

    IEnumerator playerStart(Player player)
    {
        RaycastHit hit;
        promptText.gameObject.SetActive(true);
        IEnumerator diceThrowing = ThrowDice(result => { diceResult = result; });

        yield return StartCoroutine(diceThrowing);

        promptText.gameObject.SetActive(false);
        textDiceRoll.text = diceResult.ToString();

        while (diceResult != 0)
        {
            yield return StartCoroutine(player.movePlayer());
            yield return new WaitForSeconds(0.5f);
            //Debug.Log(diceResult);
            diceResult--;
        }

        if (diceResult == 0)
        {
            //Debug.Log("dice become zero");
            if (playersTurn == 4)
            {
                //Debug.Log("playerturn set to 0");
                playersTurn = 0;
            }

            //Debug.Log("dice added by 1");
            playersTurn++;

            //Debug.Log("going next player");
            // remove this to add more function after the player movement
            if (Physics.Raycast(new Ray(player.transform.position, -transform.up), out hit, 2))
            {
                yield return StartCoroutine(Tiles.checkTile(hit, player));
            }

            //Debug.Log(hit.collider.name);

            startTurn = false;
            yield break;
        }

    }

    IEnumerator ThrowDice(Action<int> callback, bool skip = false)
    {
        // this if statment is to skip the user spacebasr input
        if (skip)
        {
            diceNumber = dice.rollDice(minThrow, maxThrow);
            callback(diceNumber);
            yield break;
        }

        //Debug.Log("Press Space");
        while (!Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForFixedUpdate();

        }

        //Debug.Log("Space Pressed");
        diceNumber = dice.rollDice(minThrow, maxThrow);
        callback(diceNumber);
        yield break;
    }

    public void updatePoint(Player player)
    {
        pointSmall.text = "Small: " + player.smallPoint.ToString();
        pointBig.text = "Big: " + player.bigPoint.ToString();
    }

}
