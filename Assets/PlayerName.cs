using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerName : MonoBehaviour
{
    public static string nameOfPlayer;
    public string saveName;

    public TMPro.TextMeshProUGUI inputText;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        SetName();
        nameOfPlayer = PlayerPrefs.GetString("name", "none");
    }
    
    public void SetName()
    {
        saveName = inputText.text;
        PlayerPrefs.SetString("name", saveName);
    }

    public static string GetName()
    {
        return nameOfPlayer;
    }
}
