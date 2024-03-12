using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class questionui : MonoBehaviour
{
    public questionManager questionManager;
    public GameObject questionCanvas;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] options;
    public string answer;
    
    // start game -> insert data to list<> -> ask question -> wait until question is answered -> remove the question from the list<>
    public question startQuestion()
    {
        if(questionManager.listQuestion().ToArray().Length == 0)
        {
            Debug.Break();
            Debug.LogWarning("end of question list inside JSON");
        }
        question Question = getQuestion();
        questionText.text = Question.Question;
        answer = Question.Answer;
        for(int i  = 0; i < options.Length; i++) 
        {
            options[i].text = Question.Options[i];
        }

        questionCanvas.SetActive(true);

        return Question;
    }
    
    public bool checkQuestion(String answer)
    {
        print(answer + " : answer from question ui");
        return true;
    }

    public void endQuestion(question Question)
    {
        print(questionManager.listQuestion().ToArray().Length - 1 + " questions left in the list");
        questionManager.deleteQuestion(Question);
        questionCanvas.SetActive(false);
    }

    public question getQuestion()
    {
        // get max number of question
        int rand = UnityEngine.Random.Range(0, questionManager.listQuestion().ToArray().Length);

        return questionManager.getQuestion(rand);
    }
}
