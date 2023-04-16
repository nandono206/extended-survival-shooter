using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryTracker : MonoBehaviour
{
    public int lastSavedSlot = -1;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}

