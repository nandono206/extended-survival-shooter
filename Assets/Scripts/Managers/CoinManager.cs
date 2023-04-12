using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int coins;

    public TMP_Text playerCoins;

    void Awake()
    {
        coins = 0;   
    }

    void Update ()
    {
        playerCoins.text = coins.ToString();
    }
}
