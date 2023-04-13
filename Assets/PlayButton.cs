using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // Include this namespace

public class PlayButton : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Don't destroy this GameObject when loading a new scene
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return null; // Wait one frame before loading the scene
        SceneManager.LoadScene("OpeningScene", LoadSceneMode.Single);
    }

}
