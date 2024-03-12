using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreboard : MonoBehaviour
{
    public EndgameRowUI EndgamerowUI;

    private GameObject scoreManagerGO;
    private ScoreManager scoreManager;

    public Sprite[] PodiumImages;

    private void Start()
    {
        scoreManagerGO = GameObject.Find("Master");
        scoreManager = scoreManagerGO.GetComponent<ScoreManager>();
        createLeaderboard(scoreManager.getHighScores().ToArray());
    }

    void createLeaderboard(Score[] scores)
    {
        for (int i = 0; i < scores.Length; i++)
        {

            var row = Instantiate(EndgamerowUI, transform).GetComponent<EndgameRowUI>();
            row.image.sprite = PodiumImages[i];
            row.name.text = scores[i].name;
            row.bigPoint.text = scores[i].bigPoint.ToString();
            row.smallPoint.text = scores[i].smallPoint.ToString();
        }
    }

}
