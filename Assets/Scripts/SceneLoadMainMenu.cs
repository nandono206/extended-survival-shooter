using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadMainMenu : MonoBehaviour
{
    void OnEnable()
    {
        //this will be used to load opening scene 2 right after opening scene 1
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }
}