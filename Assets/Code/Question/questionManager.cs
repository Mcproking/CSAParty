using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionManager : MonoBehaviour
{
    private questiondb qDB;

    private void Awake()
    {
        qDB = new questiondb();
    }

    public void addQuestion(question question)
    {
        //print("added question");
        qDB.questionList.Add(question);
    }

    public void deleteQuestion(question question)
    {
        qDB.questionList.Remove(question);
    }

    public IEnumerable<question> listQuestion()
    {
        return qDB.questionList;
    }

    public question getQuestion(int questionNumber)
    {
        return qDB.questionList[questionNumber];
    }

}
