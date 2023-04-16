using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneTracker : MonoBehaviour
{
    public bool isCutScene = false;
    public string saveTitle;
    public int questIndex;
    public float score;
    public int health;
    public int coins;
    public bool isShotgunAvailable;
    public bool isSwordAvailable;
    public bool isBowAvailable;
    public bool isGunUpgraded;
    public bool isShotgunUpgraded;
    public bool isSwordUpgraded;
    public bool isBowUpgraded;
    public int petIndex;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
