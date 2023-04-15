using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Scoreboard
{
    public string username;
    public float score;

    public Scoreboard(string username, float score)
    {
        this.username = username;
        this.score = score;
    }
}
