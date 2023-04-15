using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardData scoreboardDatas;
    public GameObject questRewardUI;
    static bool isShowingScoreboardUI = false;

    void Awake()
    {
        string path = Application.persistentDataPath + "/scoreboard.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            scoreboardDatas = JsonUtility.FromJson<ScoreboardData>(json);
        }
        
        if (scoreboardDatas == null)
        {
            scoreboardDatas = new ScoreboardData();
        }
    }

    void Update()
    {
        if (isShowingScoreboardUI)
        {
            ShowScoreboardUI();

            isShowingScoreboardUI = false;
        }
    }

    public static IEnumerable<Scoreboard> GetHighestScoreboards()
    {
        return scoreboardDatas.scores.OrderBy(x => x.score);
    }

    public static void AddScore(Scoreboard score)
    {
        scoreboardDatas.scores.Add(score);
        
        for (int i = 0; i < scoreboardDatas.scores.Count; i++)
        {
            string minutes = Mathf.Floor(scoreboardDatas.scores[i].score / 60).ToString("00");
            string seconds = (scoreboardDatas.scores[i].score % 60).ToString("00");
            Debug.Log(scoreboardDatas.scores[i].username + " " + string.Format("{0}:{1}", minutes, seconds));
        }
    }

    public static void SaveScoreboard()
    {
        string path = Application.persistentDataPath + "/scoreboard.json";
        string json = JsonUtility.ToJson(scoreboardDatas);
        System.IO.File.WriteAllText(path, json);

        isShowingScoreboardUI = true;
    }

    public void ShowScoreboardUI()
    {
        questRewardUI.SetActive(true);
    }
}
