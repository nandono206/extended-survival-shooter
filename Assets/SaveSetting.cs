using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSetting : MonoBehaviour
{
    public InputField inputNameField;

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");
            inputNameField.text = savedName;
        }
    }


    public void SaveName()
    {
        if (inputNameField != null && !string.IsNullOrEmpty(inputNameField.text))
        {
            string playerName = inputNameField.text;
            PlayerPrefs.SetString("PlayerName", playerName);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Input Field is empty or null");
        }
    }
}
