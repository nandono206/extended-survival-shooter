using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Header ("Quest Configuration")]
    public string title;
    public string description;
    public int coinReward;

    [Header ("Zombear Configuration")]
    public bool zombearIsSpawnAfterDeath;
    public float zombearSpawnTime;
    public int zombearSpawnNumber;
    
    [Header ("Zombunny Configuration")]
    public bool zombunnyIsSpawnAfterDeath;
    public float zombunnySpawnTime;
    public int zombunnySpawnNumber;
    
    [Header ("Hellephant Configuration")]
    public bool hellephantIsSpawnAfterDeath;
    public float hellephantSpawnTime;
    public int hellephantSpawnNumber;

    [Header("Boss Configuration")]
    public bool bossIsSpawnAfterDeath;
    public float bossSpawnTime;
    public int bossSpawnNumber;
}
