using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SaveState
{    // string playerName;
    public string saveTitle;
    public int questIndex;
    public float score;
    public int health;
    public int coins;
    public bool isShotgunAvailable;
    public bool isSwordAvailable;
    public bool isBowAvailable;
    public int petIndex;
}

public class SaveManager : MonoBehaviour
{
    private SaveState[] SaveSlots = new SaveState[3];
    private SaveState[] LoadSlots = new SaveState[3];
    [SerializeField]
    private GameObject SaveUI;
    [SerializeField]
    private GameObject LoadPanel;
    [SerializeField]
    private QuestManager QuestManager;
    [SerializeField]
    private ShopManager ShopManager;
    [SerializeField]
    private ScoreManager ScoreManager;
    [SerializeField]
    private PlayerHealth PlayerHealth;
    [SerializeField]
    private PlayerShooting PlayerShooting;
    [SerializeField]
    private CoinManager CoinManager;
    [SerializeField]
    private Spawner Spawner;
    [SerializeField]
    private GameObject[] SlotButtons = new GameObject[3];
    [SerializeField]
    private GameObject[] SlotInputs = new GameObject[3];
    private GameObject[] SlotFields = new GameObject[3];
    private GameObject[] SlotSubmits = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        CutSceneTracker cutSceneTracker = GameObject.Find("CutSceneTracker").GetComponent<CutSceneTracker>();
        if (cutSceneTracker.isCutScene)
        {
            QuestManager.currentQuestIdx = cutSceneTracker.questIndex;
            ScoreManager.score = cutSceneTracker.score;
            PlayerHealth.currentHealth = cutSceneTracker.health;
            CoinManager.coins = cutSceneTracker.coins;
            PlayerShooting.isShotgunAvailable = cutSceneTracker.isShotgunAvailable;
            PlayerShooting.isSwordAvailable = cutSceneTracker.isSwordAvailable;
            PlayerShooting.isBowAvailable = cutSceneTracker.isBowAvailable;
            if (cutSceneTracker.petIndex != -1)
            {
                Spawner.SpawnObject(cutSceneTracker.petIndex);
            }
            cutSceneTracker.isCutScene = false;
            QuestManager.StartQuest();
        }
        else
        {
            LoadMenuTracker loadMenuTracker = GameObject.Find("LoadMenuTracker").GetComponent<LoadMenuTracker>();
            if (loadMenuTracker.isLoadMenu)
            {
                LoadPanel.SetActive(true);
                SaveUI.SetActive(true);
                loadMenuTracker.isLoadMenu = false;
            }
            else
            {
                LoadPanel.SetActive(false);
                SaveUI.SetActive(false);
                QuestManager.StartQuest();
            }
        }
        for (int i = 0; i < 3; i++)
        {
            SaveSlots[i] = new SaveState();
            SlotFields[i] = SlotInputs[i].transform.GetChild(0).gameObject;
            SlotSubmits[i] = SlotInputs[i].transform.GetChild(1).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveSlot(int slot)
    {
        // get TMP text field and save to saveTitle
        SaveSlots[slot].saveTitle = SlotFields[slot].GetComponent<TMP_InputField>().text;
        SlotButtons[slot].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = SaveSlots[slot].saveTitle;
        SaveSlots[slot].questIndex = QuestManager.currentQuestIdx;
        SaveSlots[slot].score = ScoreManager.score;
        SaveSlots[slot].health = PlayerHealth.currentHealth;
        SaveSlots[slot].coins = CoinManager.coins;
        SaveSlots[slot].isShotgunAvailable = PlayerShooting.isShotgunAvailable;
        SaveSlots[slot].isSwordAvailable = PlayerShooting.isSwordAvailable;
        SaveSlots[slot].isBowAvailable = PlayerShooting.isBowAvailable;
        SaveSlots[slot].petIndex = Spawner.petIndex;
        // to json, then save as proprietary file
        string json = JsonUtility.ToJson(SaveSlots[slot]);
        string path = Application.persistentDataPath + "/save" + slot + ".json";
        System.IO.File.WriteAllText(path, json);
        // hide input field
        SlotInputs[slot].SetActive(false);
    }

    public void LoadSlot(int slot)
    {
        // load from proprietary file
        string path = Application.persistentDataPath + "/save" + slot + ".json";
        string json = System.IO.File.ReadAllText(path);
        LoadSlots[slot] = JsonUtility.FromJson<SaveState>(json);
        // load into game
        QuestManager.currentQuestIdx = LoadSlots[slot].questIndex;
        ScoreManager.score = LoadSlots[slot].score;
        PlayerHealth.currentHealth = LoadSlots[slot].health;
        CoinManager.coins = LoadSlots[slot].coins;
        PlayerShooting.isShotgunAvailable = LoadSlots[slot].isShotgunAvailable;
        PlayerShooting.isSwordAvailable = LoadSlots[slot].isSwordAvailable;
        PlayerShooting.isBowAvailable = LoadSlots[slot].isBowAvailable;
        if (LoadSlots[slot].petIndex != -1)
        {
            Spawner.SpawnObject(LoadSlots[slot].petIndex);
        }
        // hide load slots panel
        LoadPanel.SetActive(false);
        SaveUI.SetActive(false);
        QuestManager.StartQuest();
        ShopManager.RegenerateShop();
    }

    public void goToCutScene()
    {
        CutSceneTracker cutSceneTracker = GameObject.Find("CutSceneTracker").GetComponent<CutSceneTracker>();
        cutSceneTracker.questIndex = QuestManager.currentQuestIdx;
        cutSceneTracker.score = ScoreManager.score;
        cutSceneTracker.health = PlayerHealth.currentHealth;
        cutSceneTracker.coins = CoinManager.coins;
        cutSceneTracker.isShotgunAvailable = PlayerShooting.isShotgunAvailable;
        cutSceneTracker.isSwordAvailable = PlayerShooting.isSwordAvailable;
        cutSceneTracker.isBowAvailable = PlayerShooting.isBowAvailable;
        cutSceneTracker.petIndex = Spawner.petIndex;
        cutSceneTracker.isCutScene = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_Tambahan");
    }
}
