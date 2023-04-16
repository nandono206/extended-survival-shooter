using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveStateOld : ScriptableObject
{
    public string saveTitle;
    public int questIndex;
    public float score;
    public int health;
    public int coins;
    public bool isShotgunAvailable;
    public bool isSwordAvailable;
    public bool isBowAvailable;
    // int petIndex;
}
