using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;
    public questionManager QuestionManager;
    void Start()
    {
        //print("start json");
        questions questionsInJSON = JsonUtility.FromJson<questions>(jsonFile.text);

        foreach(question question in questionsInJSON.Questions)
        {
            QuestionManager.addQuestion(question);
        }

    }

}
