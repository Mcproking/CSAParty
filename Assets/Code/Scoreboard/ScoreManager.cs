using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;
    void Awake()
    {
        sd = new ScoreData();
    }

    public IEnumerable<Score> arrangeName()
    {
        return sd.scores.OrderBy(s => s.name);
    }

    public IEnumerable<Score> getHighScores()
    {
        return sd.scores.OrderByDescending(x => x.smallPoint).OrderByDescending(x => x.bigPoint) ;
    }

    /// <summary>
    /// AddScore(String PlayerName, int bigPoint, int smallPoint)
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }

    public void UpdateBigPoint(int BigPoint, int playerTurn)
    {
        sd.scores[playerTurn].bigPoint = BigPoint;
    }

    public void UpdateSmallPoint(int SmallPoint, int PlayerTurn)
    {
        sd.scores[PlayerTurn].smallPoint = SmallPoint;
    }
}
