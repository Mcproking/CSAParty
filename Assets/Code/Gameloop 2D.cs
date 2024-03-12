using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gameloop2D : MonoBehaviour
{
    public master2D master;
    public Dice2D dice;
    public returnButtonText[] ReturnButtonText;
    public TextMeshProUGUI RoundText;


    [Header("Score Related")]
    public GameObject ConvertGUI;
    public ScoreUI scoreUI;
    public ScoreManager scoreManager;

    [Header("Question Related")]
    public questionui questionUI;
    public GameObject correctBox;
    public GameObject worngBox;

    [Header("Camera Related")]
    public Transform OriginalCameraPos;

    [Header("Timer")]
    public Image timerImage;
    public TextMeshProUGUI timerText;

    string buttonText;
    

    int diceResult;

    bool convert_userDecision;
    bool convert_buttonPressed;

    bool question_buttonPressed;
    float timeTaken;

    // start player turn > throw dice > auto move > split direction (wait user input) > go to user input direction > contiune until finish step > check home tile > [yes](see user want to convert) [no](ask question and give points) > sort leaderboard > next player

    public IEnumerator currentPlayerMove(Player2D player, string PlayerTile)
    {
        if (master.getPlayerStart())
        {
            RoundText.text = "Round " + master.getCurrentRound() + "/" + master.MaxRound;
            yield return StartCoroutine(moveCamera(player.GetComponentInChildren<Transform>().Find("Camera Pos").position));

            //print("press space");
            while (!Input.GetKey(KeyCode.Space))
            {
                //print("wating space press");
                yield return new WaitForFixedUpdate();
            }

            //print("start dice roll");
            yield return StartCoroutine(dice.rollDice(result => { diceResult = result; }));

            while (diceResult != 0)
            {
                //print("moving dice " + diceResult.ToString());
                yield return StartCoroutine(player.MovePlayer());
                //yield return new WaitForSeconds(0.5f);
                diceResult--;
            }

            if (diceResult == 0)
            {
                //print("dice finsh");
                string tileHit;
                if (Physics.Raycast(new Ray(player.transform.position, transform.forward), out RaycastHit hit, 1f))
                {
                    // check if player standing on thier own spawn tile
                    tileHit = hit.collider.name;
                    if (tileHit == PlayerTile)
                    {
                        ConvertGUI.gameObject.SetActive(true);

                        while (!convert_buttonPressed)
                        {
                            yield return new WaitForFixedUpdate();
                        }

                        if (!convert_userDecision) // user press no
                        {
                            ConvertGUI.gameObject.SetActive(false);
                            convert_userDecision = convert_buttonPressed = false;
                            yield break;
                        }

                        if (player.getSmallPoint() < 5) // user press yes, but smallpoint less than 5
                        {
                            ConvertGUI.gameObject.SetActive(false);
                            print("user not enough small point");
                            yield break;
                        }


                        if (convert_userDecision && convert_buttonPressed)
                        {
                            // show the canvas convert prompt

                            // convert
                            int smallPoint = player.getSmallPoint();
                            int bigPoint = player.getBigPoint();
                            int finalSmallPoint;
                            int finalBigPoint;

                            finalSmallPoint = smallPoint % master.pointConvertRatio;
                            finalBigPoint = ((smallPoint - finalSmallPoint) / master.pointConvertRatio) + bigPoint;

                            player.setSmallPoint(finalSmallPoint);
                            player.setBigPoint(finalBigPoint);

                            ConvertGUI.gameObject.SetActive(false);
                            convert_buttonPressed = convert_buttonPressed = false;
                        }
                    }
                    else
                    {
                        //print("ask question");
                        question Question = questionUI.startQuestion();
                        bool answerCorrect = false;

                        yield return StartCoroutine(startTimer());
                        //print("finish timer");
                        //print(timeTaken >= master.questionTime);
                        while (!question_buttonPressed)
                        {
                            //print("wait user input");

                            if (timeTaken >= master.questionTime || question_buttonPressed)
                            {
                                //print("pass time");
                                question_buttonPressed = true;
                            }

                            yield return new WaitForFixedUpdate();
                        }

                        foreach (returnButtonText option in ReturnButtonText)
                        {
                            if (option.getReturnedText() != null)
                            {
                                buttonText = option.getReturnedText();
                                break;
                            }
                        }

                        if ( ( question_buttonPressed && (buttonText != null) ) || timeTaken >= master.questionTime)
                        {
                            // corutine to check the answer is correct with return of bool
                            // if the answer is correct, then give score multiplyer, if is worng no multipyer and see correct answer

                            yield return StartCoroutine(checkAnswer(result => { answerCorrect = result; }, buttonText));

                            if (answerCorrect)
                            {
                                //print("answer correct");
                                //print("add point with multiplyer");
                                correctBox.SetActive(true);
                                yield return new WaitForSeconds(1.5f);
                                player.setSmallPoint(player.getSmallPoint() + (master.pointAchive * master.pointMultipler));
                                correctBox.SetActive(false);
                            }
                            else
                            {
                                //print("answer worng / no give answer");
                                worngBox.SetActive(true);
                                yield return new WaitForSeconds(1.5f);
                                //print(buttonText);

                                if (timeTaken >= master.questionTime )
                                {
                                    //no point are given.
                                    //print("no point inside wrong answer");
                                    player.setSmallPoint(player.getSmallPoint());
                                }
                                else
                                {
                                    //add point when user answer with worng asnwer
                                    //print("add point without multiplyer");
                                    player.setSmallPoint(player.getSmallPoint() + (master.pointAchive * 1));
                                }

                                worngBox.SetActive(false);
                            }

                            // remove the question from the list
                            questionUI.endQuestion(Question);

                            // reset the button 
                            question_buttonPressed = false;

                            // reset time taken
                            timeTaken = 0;
                        }

                    }
                }

                // sort leaderboard and update leaderbaord
                //print("update score to scoredata");
                scoreManager.UpdateSmallPoint(player.getSmallPoint(), master.getPlayerTurn() - 1);
                scoreManager.UpdateBigPoint(player.getBigPoint(), master.getPlayerTurn() - 1);

                //print("recreate leaderboard");
                scoreUI.sortByHighScore();

                if (master.getPlayerTurn() == 4)
                {
                    master.setPlayerTurn(1);
                    yield return StartCoroutine(returnCamera());
                    yield return new WaitForSeconds(3);
                    master.setPlayerStart(false);
                    roundCheck();
                    yield break;
                }

                master.setPlayerTurn(master.getPlayerTurn() + 1);

            }

            master.setPlayerStart(false);
            roundCheck();
            yield break;

        }
    }


    public void PointConvert_True()
    {
        convert_buttonPressed = true;
        convert_userDecision = true;
    }

    public void PointConvert_False()
    {
        convert_buttonPressed = true;
    }

    public void questionButton()
    {
        question_buttonPressed = true;
    }

    public void roundCheck()
    {
        if (master.getLoopPlayer() == 4)
        {
            master.addRound();
            master.resetLoopPlayer();
        }
        else
        {
            master.addLoopPlayer();
        }
    }

    IEnumerator checkAnswer(Action<bool> callback,string choosenAnswer)
    {
        if(questionUI.answer == choosenAnswer)
        {
            callback(true);
            yield break;
        }

        callback(false); 
        yield break;
    }

    public IEnumerator moveCamera(Vector3 playerCameraPos)
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
        while(Vector3.Distance(camera.transform.position, playerCameraPos) > 0.005f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 1, master.cameraMoveSpeed * Time.deltaTime);
            camera.transform.position = Vector3.Lerp(camera.transform.position, playerCameraPos, master.cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        camera.transform.position = playerCameraPos;

    }

    public IEnumerator returnCamera()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector3 originalPos = OriginalCameraPos.position;
        //print(camera.transform.position);
        //print(originalPos);

        while(Vector3.Distance(camera.transform.position, originalPos) > 0.005f)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 2.1f, master.cameraMoveSpeed * Time.deltaTime);
            camera.transform.position = Vector3.Lerp(camera.transform.position, originalPos, master.cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        camera.transform.position = originalPos;
    }

    public IEnumerator startTimer()
    {
        int maxTimetaken = master.questionTime;
        while(timeTaken <= maxTimetaken)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            timerText.text = (maxTimetaken - timeTaken).ToString("0");
            timerImage.fillAmount = Mathf.InverseLerp(0, maxTimetaken, maxTimetaken - timeTaken);
            //print(timeTaken);
            if (question_buttonPressed || timeTaken + 0.01f >= maxTimetaken )
            {
                //print("timer finish");
                timeTaken += 0.01f;
                yield break;
            }

            timeTaken += 0.01f;
        }
    }
}


