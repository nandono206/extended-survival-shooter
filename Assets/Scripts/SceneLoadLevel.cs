using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadLevel : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Level_01", LoadSceneMode.Single);
    }
}