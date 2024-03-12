using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Player2D : MonoBehaviour
{
    public master2D master;
    public Gameloop2D gameloop;
    public string Name;


    Ray upRay;
    Ray leftRay;
    Ray rightRay;
    Ray downRay;
    RaycastHit hit;

    bool[] directions;
    string lastDirection;


    bool leftButton;
    bool rightButton;
    bool upButton;
    bool downButton;

    //float moveSpeed = 10f; // change to take value from master

    Button[] directionButtons; // [up left, down, right]
    SpriteRenderer[] playerSprites;

    //Points
    [SerializeField]
    private int BigPoint = 0;
    [SerializeField]
    private int SmallPoint = 0;


    public void Awake()
    {
        //print(this.name + "player2d awake");
        if(Name == ""  || Name == null)
        {
            Name = transform.name;
        }

        // [up,left,down,right]
        directions = new bool[4];
        directionButtons = this.GetComponentsInChildren<Button>(true);
        playerSprites = this.GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space)) 
        //{
        //    StartCoroutine(MovePlayer());
        //}

        Debug.DrawRay(transform.position, transform.up);
        Debug.DrawRay(transform.position, -transform.up);
        Debug.DrawRay (transform.position, transform.right);
        Debug.DrawRay(transform.position, -transform.right);
        Debug.DrawRay(transform.position, transform.forward);
    }

    /// 
    /// Code below here are for player Points
    /// 

    public int getBigPoint()
    {
        return BigPoint;
    }

    public void setBigPoint(int input)
    {
        BigPoint = input;
    }

    public int getSmallPoint()
    {
        return SmallPoint;
    }

    public void setSmallPoint(int input)
    {
        SmallPoint = input;
    }


    /// 
    /// Code below here are for player movement
    /// 
    

    void castRay()
    {
        //Debug.Log("cast ray");

        upRay = new Ray(transform.position, transform.up);
        downRay = new Ray (transform.position, -transform.up);
        rightRay = new Ray(transform.position, transform.right);   
        leftRay = new Ray(transform.position, -transform.right);
    }

    void checkDirection()
    {
        //Debug.Log("Check direction");

        if (Physics.Raycast(upRay, 0.5f)) { directions[0] = true; }
        if (Physics.Raycast(leftRay, 0.5f)) { directions[1] = true; }
        if (Physics.Raycast(downRay, 0.5f)) { directions[2] = true; }
        if (Physics.Raycast(rightRay, 0.5f)) { directions[3] = true; }
    }

    void checkLastDirection()
    {
        //Debug.Log("check last direction");
        if(lastDirection == "" || lastDirection == null)
        {
            //Debug.Log("last direction empty");
            return;
        }

        //Debug.Log("last direction avalible");

        // based on last direction, set the oppiside direction to false based on last direction
        // [up,left,down,right]
        switch (lastDirection)
        {
            case "up":
                directions[2] = false;
                return;
            case "left":
                directions[3] = false;
                return;
            case "down":
                directions[0] = false;
                return;
            case "right":
                //Debug.Log("set left false");
                directions[1] = false;
                return;
        }
    }

    

    IEnumerator movingPlayer(Vector3 targetPosition)
    {

        while (Vector3.Distance(transform.position, targetPosition) > 0.005f)
        { 
            transform.position = Vector3.Lerp(transform.position, targetPosition, master.moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return StartCoroutine(gameloop.moveCamera(this.GetComponentInChildren<Transform>().Find("Camera Pos").position));

        Array.Clear(directions, 0, directions.Length);
        transform.position = targetPosition;
    }

    IEnumerator movingPath(int directionNumber)
    {
        switch (directionNumber)
        {
            case 0:
                //go up
                if (Physics.Raycast(upRay, out hit, 0.5f))
                {
                    Vector3 tilePosition = hit.transform.position;
                    lastDirection = "up";
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
                yield break;
            case 1:
                //go left
                if (Physics.Raycast(leftRay, out hit, 0.5f))
                {
                    Vector3 tilePosition = hit.transform.position;
                    lastDirection = "left";
                    foreach(SpriteRenderer s in playerSprites)
                    {
                        s.flipX = true;
                    }
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
                yield break;
            case 2:
                //go down
                if (Physics.Raycast(downRay, out hit, 0.5f))
                {
                    Vector3 tilePosition = hit.transform.position;
                    lastDirection = "down";
                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
                yield break;
            case 3:
                //go right
                if (Physics.Raycast(rightRay, out hit, 0.5f))
                {
                    Vector3 tilePosition = hit.transform.position;
                    lastDirection = "right";
                    foreach (SpriteRenderer s in playerSprites)
                    {
                        s.flipX = false;
                    }

                    yield return StartCoroutine(movingPlayer(tilePosition));
                }
                yield break;
            case -1:
                //if all cases above is not assigned
                yield break;
        }
    }

    IEnumerator waitPlayerButtonPress()
    {
        //Debug.Log("Start loop");
        while(!(leftButton || rightButton || upButton || downButton))
        {
            yield return new WaitForSecondsRealtime(0.01f);

            if(leftButton || rightButton || upButton || downButton)
            {
                //Debug.Log("Loop breal");
                break;
            }
        }

        foreach(Button button in directionButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (upButton)
        {
            // move up
            yield return StartCoroutine(movingPath(0));
        }
        if (leftButton)
        {
            // move left
            yield return StartCoroutine(movingPath(1));
        }
        if (downButton)
        {
            // move up
            yield return StartCoroutine(movingPath(2));
        }
        if (rightButton)
        {
            // move up
            yield return StartCoroutine(movingPath(3));
        }

        upButton = leftButton = downButton = rightButton = false;
    }

    public void buttonLeftPressed()
    {
        //Debug.Log("left press");
        leftButton = true;
    }
    public void buttonRightPressed()
    {
        //Debug.Log("right press");
        rightButton = true;
    }
    public void buttonUpPressed()
    {
        //Debug.Log("up press");
        upButton = true;
    }
    public void buttonDownPressed()
    {
        //Debug.Log("down press");
        downButton = true;
    }


    public IEnumerator MovePlayer()
    {
        int paths = 0;

        castRay();
        checkDirection();
        checkLastDirection();

        foreach (bool direction in directions)
        {
            if (direction)
            {
                paths++;
            }
        }

        //Debug.Log(paths);

        if (paths == 0)
        {
            Debug.LogWarning("No path, impossible!!");
            yield break;
        }

        if (paths == 1)
        {
            int path = -1;

            for (int i = 0; i < directions.Length; i++)
            {
                if (directions[i])
                {
                    path = i;
                    break;
                }
            }

            yield return StartCoroutine(movingPath(path));
        }

        if (paths > 1)
        {
            int[] path = new int[0];
            for (int i = 0; i < directions.Length; i++)
            {
                if (directions[i])
                {
                    Array.Resize(ref path, path.Length + 1);

                    path[directions.Length - (directions.Length - path.Length + 1)] = i;

                    //Debug.Log(directions.Length - (directions.Length - path.Length));
                    //Debug.Log(path.Length);
                }
            }

            foreach (int i in path)
            {
                //Debug.Log(i);
                switch (i)
                {
                    case 0:
                        // show up button
                        directionButtons[0].gameObject.SetActive(true);
                        break;
                    case 1:
                        //show left button
                        directionButtons[1].gameObject.SetActive(true);
                        break;
                    case 2:
                        //show right button
                        directionButtons[2].gameObject.SetActive(true);
                        break;
                    case 3:
                        //show down button
                        directionButtons[3].gameObject.SetActive(true);
                        break;
                }
            }

            yield return StartCoroutine(waitPlayerButtonPress());

        }

        //yield return null;
    }


}
