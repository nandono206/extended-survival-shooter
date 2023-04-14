using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreboardData
{
    public List<Scoreboard> scores;
    
    public ScoreboardData()
    {
        scores = new List<Scoreboard>();
    }
}
