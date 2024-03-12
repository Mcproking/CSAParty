using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;

    void Start()
    {
        //print("Score ui started");
        //scoreManager.AddScore(new Score("Banana", 3, 4));
        //scoreManager.AddScore(new Score("Bear", 4, 20));
        //scoreManager.AddScore(new Score("Eraser", 2, 20));
        //scoreManager.AddScore(new Score("Luilui", 0, 40));

        var scores = scoreManager.arrangeName().ToArray();
        createLeaderboard(scores);
    }

    public void sortByHighScore()
    {
        var scores = scoreManager.getHighScores().ToArray();
        updateLeaderboard(scores);
    }

    void updateLeaderboard(Score[] scores)
    {
        //print("updating leaderboard");  
        RowUI[] rows = gameObject.GetComponentsInChildren<RowUI>();
        foreach (RowUI row in rows)
        {
            //print("destory score");
            Destroy(row.gameObject);
        }
        createLeaderboard(scores);
    }


    void createLeaderboard(Score[] scores)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.name.text = scores[i].name;
            row.bigPoint.text = scores[i].bigPoint.ToString();
            row.smallPoint.text = scores[i].smallPoint.ToString();
        }
    }

    

    // below part is get the sort by highest score
    
}
