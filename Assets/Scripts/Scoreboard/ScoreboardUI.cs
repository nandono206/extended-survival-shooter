using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    public ScoreboardRowUI scoreboardRowUI;

    void Start()
    {
        var scores = ScoreboardManager.GetHighestScoreboards().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(scoreboardRowUI, transform).GetComponent<ScoreboardRowUI>();
            row.rank.text = (i + 1).ToString();
            row.username.text = scores[i].username;

            string minutes = Mathf.Floor(scores[i].score / 60).ToString("00");
            string seconds = (scores[i].score % 60).ToString("00");
            row.score.text = string.Format("{0}:{1}", minutes, seconds);;
        }
    }
}
