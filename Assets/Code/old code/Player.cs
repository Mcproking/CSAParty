using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Point")]
    public int smallPoint = 0;
    public int bigPoint = 0;


    Ray frontRay;
    Ray leftRay;
    Ray rightRay;
    RaycastHit hit;

    Canvas directionButtons;
    Button[] buttonDirections;

    bool leftClicked = false;
    bool rightClicked = false;
    bool frontClicked = false;

    IEnumerator waitingUserButton;
    IEnumerator startCountDown;


    private bool[] directions;

    //[Header("Refernce code")]
    public master Master;

    // player movement through the board, selection of direction
    void Start()
    {
        updateRaycast();
        directions = new bool[3];
        directionButtons = this.GetComponentInChildren<Canvas>();
        buttonDirections = directionButtons.GetComponentsInChildren<Button>(true);

        //if (Physics.Raycast(frontRay, out RaycastHit hit, 3))
        //{
        //    //Vector3 objecthitem = hit.transform.position;
        //    //Debug.Log(transform.name +" sees "+objecthitem);
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject sprite;
        Camera camera;
        camera = this.GetComponentInChildren<Camera>();
        sprite = GameObject.Find("Sprite");
        camera.transform.LookAt(sprite.transform);

        // front: red
        // left: blue
        // right: green
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red);
        Debug.DrawRay(transform.position, transform.right * -3, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 3, Color.green);
        Debug.DrawRay(transform.position, transform.up * -2, Color.green);
    }

    void updateRaycast() // this will be call everytime when the player is moving, and once when the script start
    {
        frontRay = new Ray(transform.position, transform.forward);
        leftRay = new Ray(transform.position, -transform.right);
        rightRay = new Ray(transform.position, transform.right);

    }

    public IEnumerator movePlayer() // need to called from master
    {
        //Debug.Log("move player");
        int paths = 0;

        if (Physics.Raycast(leftRay, 3)) { directions[0] = true; }
        if (Physics.Raycast(frontRay, 3)) { directions[1] = true; }
        if (Physics.Raycast(rightRay, 3)) { directions[2] = true; }


        // check for how many path that the thing can take
        foreach(bool direction in directions)
        {
            if (direction)
            {
                paths++;
            }
        }

        // check for no direction. impossible to get but guard it 1st
        if(paths  == 0)
        {
            Debug.LogWarning("No path, impossible!!");
            yield break;
        }
        
        if (paths > 1)
        {
            bool playerChoice = true;

            // show avaliable path > wait for user input > rotate and go
            int pathAvaliable = Array.IndexOf(directions, false);

            // so the pathAvaliable will show which path cannot be taken
            // using that method and elimanation pattern to know which path is avalible instead of check one by one
            // 0 = fwd,right || 1 = left,right || 2 = fwd,left

            // Debug.Log(transform.name + " saids " + (-pathAvaliable-2) + "path avaliable");

            // buttonDirections is array and based on [left, fwd, right]

            if (pathAvaliable == -1)
            {
                Debug.LogWarning("Impossible warning at pathAvaliable");
                yield break;
            }

            if (pathAvaliable == 0) // fwd, right
            {
                buttonDirections[1].gameObject.SetActive(true);
                buttonDirections[2].gameObject.SetActive(true);
            }

            if (pathAvaliable == 1) // left, right
            {
                buttonDirections[0].gameObject.SetActive(true);
                buttonDirections[2].gameObject.SetActive(true);
            }

            if(pathAvaliable == 2) // left, fwd
            {
                buttonDirections[0].gameObject.SetActive(true);
                buttonDirections[1].gameObject.SetActive(true);
            }

            if (playerChoice)
            {
                // waiting for player choice, if no player choice then randomize
                // if got player choice, then just do the movement inside coroutne **hopefully it works
                waitingUserButton = waitForButton(Master.CountdownWaitingPlayerInput);
                yield return StartCoroutine(waitingUserButton);
            }
        }



        // move player to next tile, only called when only the possible path is one
        if (paths == 1)
        {
            int pathDirection = Array.FindIndex(directions, element => element);

            if (pathDirection == -1) {
                Debug.LogWarning("Impossible warning at pathDirection");
                yield break;
            }

            // if front path is true, move front
            if (pathDirection == 1)
            {
                if(Physics.Raycast(frontRay, out hit, 3)) { 
                    Vector3 tilePosition = hit.transform.position;
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
            }

            // if left path is true, turn left and move front
            if (pathDirection == 0)
            {
                if(Physics.Raycast(leftRay, out hit, 3))
                {
                    Vector3 tilePosition = hit.transform.position;
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
            }

            // if right path is true, trun right and move front
            if (pathDirection == 2)
            {
                if(Physics.Raycast(rightRay, out hit, 3))
                {
                    Vector3 tilePosition = hit.transform.position;
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
            }
        }
    }


    public void leftClick()
    {
        //Debug.Log("left clicked");
        leftClicked = true;
    }

    public void rightClick()
    {
        //Debug.Log("right cliked");
        rightClicked = true;
    }

    public void frontClick()
    {
        //Debug.Log("front clicked");
        frontClicked = true;
    }

    IEnumerator waitForButton(float timer)
    {
        startCountDown = countdownTimer(timer);
        yield return StartCoroutine(startCountDown);

        if (!leftClicked && !rightClicked && !frontClicked)
        {
            //Debug.Log("nothing pressed");
            yield break;
        }

        //Debug.Log("button clicked");
        foreach (Button button in buttonDirections)
        {
            button.gameObject.SetActive(false);
        }
        StopCoroutine(countdownTimer(timer));
        yield return null;

        if (frontClicked)
        {
            if (Physics.Raycast(frontRay, out hit, 3)){
                Vector3 tilePosition = hit.transform.position;
                yield return StartCoroutine(movingPlayer(tilePosition));
                frontClicked = false;
            }
        }

        if (leftClicked)
        {
            if (Physics.Raycast(leftRay, out hit, 3))
            {
                Vector3 tilePosition = hit.transform.position;
                yield return StartCoroutine(movingPlayer(tilePosition));
                leftClicked = false;
            }
        }

        if (rightClicked)
        {
            if (Physics.Raycast(rightRay, out hit, 3))
            {
                Vector3 tilePosition = hit.transform.position;
                yield return StartCoroutine(movingPlayer(tilePosition));
                rightClicked = false;
            }
        }

        yield return null;
    }

    IEnumerator countdownTimer(float timer)
    {
        float timePass = 0;
        Master.countdownTimer.gameObject.SetActive(true);

        //do to 10 sec count down
        while (!(leftClicked || rightClicked || frontClicked))
        {
            yield return new WaitForSeconds(0.1f);
            timePass += 0.1f;
            //Debug.Log("timer: "+timePass);
            Master.countdownTimer.text = "Timer \n" + (timer - timePass).ToString("0.0");

            if ((timePass - timer) > 0.05f)            
            {
                //Debug.Log("coundown finish");
                Master.countdownTimer.text = "Timer\n" + Master.CountdownWaitingPlayerInput.ToString();
                Master.countdownTimer.gameObject.SetActive(false);
                break;
            }
        }

        if (leftClicked | rightClicked | frontClicked) {
            //Debug.Log("something pressed");
            Master.countdownTimer.text = "Timer\n" + Master.CountdownWaitingPlayerInput.ToString();
            Master.countdownTimer.gameObject.SetActive(false);
            yield break;
        }

        //print("waited for " + Time.time);
        //print("doing random move");
         

        // player didnt press the button
        foreach (Button button in buttonDirections)
        {
            button.gameObject.SetActive(false);
        }

        int rnd = 0; 
        
        while (!directions[rnd])
        {
            yield return new WaitForSeconds(0.1f);
            rnd = UnityEngine.Random.Range(0, 2);
        }

        if (directions[rnd])
        {
            if (rnd == 1) // move fwd
            {
                if (Physics.Raycast(frontRay, out hit, 3))
                {
                    Vector3 tilePosition = hit.transform.position;
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
            }

            if (rnd == 0) // move left
            {
                if (Physics.Raycast(leftRay, out hit, 3))
                {
                    Vector3 tilePosition = hit.transform.position;
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
            }
            if (rnd == 2) // move right
            {
                if (Physics.Raycast(rightRay, out hit, 3))
                {
                    Vector3 tilePosition = hit.transform.position;
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
            }
        }

        frontClicked = leftClicked = rightClicked = false;
        yield break;
    }


    IEnumerator movingPlayer(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Master.moveSpeed * Time.deltaTime);
            yield return null;
        }

        updateRaycast();
        Array.Clear(directions, 0, directions.Length);
        transform.position = targetPosition;
    }
}
