using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static float score;
    public static bool isTimePaused = true;


    Text text;


    void Awake ()
    {
        text = GetComponent <Text> ();
        score = 0;
    }


    void Update ()
    {
        if (!isTimePaused)
        {
            score += Time.deltaTime;
        }
 
        string minutes = Mathf.Floor(score / 60).ToString("00");
        string seconds = (score % 60).ToString("00");
        
        text.text = string.Format("{0}:{1}", minutes, seconds);
    }
}
