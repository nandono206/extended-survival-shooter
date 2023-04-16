using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // Include this namespace

public class PlayButton : MonoBehaviour
{
    private LoadMenuTracker LoadMenuTracker;
    [SerializeField]
    private GameObject LoadMenuTrackerPrefab;
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Don't destroy this GameObject when loading a new scene
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void Awake()
    {
        GameObject LoadMenuTrackerObj = GameObject.Find("LoadMenuTracker(Clone)");
        if (LoadMenuTrackerObj == null)
        {
            LoadMenuTrackerObj = Instantiate(LoadMenuTrackerPrefab) as GameObject;
        }
        LoadMenuTracker = LoadMenuTrackerObj.GetComponent<LoadMenuTracker>();
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

    public void OnClickLoad()
    {
        StartCoroutine(LoadSaveSceneAsync());
    }

    IEnumerator LoadSaveSceneAsync()
    {
        LoadMenuTracker.isLoadMenu = true;
        yield return null; // Wait one frame before loading the scene
        SceneManager.LoadScene("Level_01", LoadSceneMode.Single);
    }
}
