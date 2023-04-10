using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int coins;

    TMP_Text playerCoins;

    void Awake()
    {
        playerCoins = GetComponent <TMP_Text> ();
        coins = 0;   
    }

    void Update ()
    {
        playerCoins.text = coins.ToString();
    }
}
