using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestManager : MonoBehaviour
{
    GameObject player;
    GameObject BossHealthUI;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool started = false;
    [SerializeField]
    private SaveManager SaveManager;

    [Header("Quest Configuration")]
    public List<Quest> questList;
    Quest currentActiveQuest;
    public int currentQuestIdx = 0;
    bool isPreviousQuestCompleted = false;
    float timeLeftUntilNextQuest = 0;

    [Header("Enemy Configuration")]
    public GameObject zombear;
    public GameObject zombunny;
    public GameObject hellephant;
    public GameObject Boss;
    public Transform[] zombearSpawnPoints;
    public Transform[] zombunnySpawnPoints;
    public Transform[] hellephantSpawnPoints;
    public Transform[] bossSpawnPoints;

    int currentZombearSpawnNumber;
    int currentZombunnySpawnNumber;
    int currentHellephantSpawnNumber;
    int currentBossSpawnNumber;
    int currentZombearSpawned;
    int currentZombunnySpawned;
    int currentHellephantSpawned;
    int currentBossSpawned;
    public static int currentZombearKilled;
    public static int currentZombunnyKilled;
    public static int currentHellephantKilled;
    public static int currentBossKilled;

    [Header("UI Configuration")]
    public GameObject questRewardUI;
    public TMP_Text questRewardIndex;
    public TMP_Text questRewardTitle;
    public TMP_Text questRewardCoins;
    public GameObject questCountdownUI;
    public TMP_Text questCountdownIndex;
    public TMP_Text questCountdownTime;
    bool questRewardShown = false;
    bool nextQuestShown;
    bool startedSavedGame = false;
    public Button closeButton;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerShooting = player.GetComponentInChildren<PlayerShooting>();
        BossHealthUI = GameObject.Find("BossHealthUI");
        BossHealthUI.SetActive(false);
        
    }

    public void StartQuest()
    {
        started = true;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(onCloseButton);

        // Start first quest
        currentActiveQuest = questList[currentQuestIdx];

        if (currentQuestIdx == 3)
        {
            BossHealthUI.SetActive(true);
        }

        if (currentQuestIdx != 0)
        {
            ShopManager.isShopAvailable = true;
            questRewardShown = false;
            nextQuestShown = true;

            if (currentQuestIdx >= questList.Count)
            {
                Debug.Log("Final boss is defeated!");
                SaveToScoreboard();

                RedirectToEnding();

                return;
            }

            currentActiveQuest = questList[currentQuestIdx];
            questCountdownIndex.text = "Quest " + (currentQuestIdx + 1) + " starting in:";

            timeLeftUntilNextQuest = 15f;

            startedSavedGame = true;
        }
        else
        {
            // Show quest UI
            // If key is clicked, start next quest
            GenerateQuest();
        }
    }

    void Update()
    {
        if (!started)
        {
            return;
        }
        
        if (timeLeftUntilNextQuest > 0f)
        {
            questCountdownUI.SetActive(true);
            timeLeftUntilNextQuest -= Time.deltaTime;
            questCountdownTime.text = Mathf.CeilToInt(timeLeftUntilNextQuest).ToString();
        }
        else
        {
            questCountdownUI.SetActive(false);
            timeLeftUntilNextQuest = 0f;
        }

        if (playerHealth.currentHealth <= 0f || (currentQuestIdx >= questList.Count && !questRewardShown))
        {
            return;
        }

        isPreviousQuestCompleted = questCompleted();
        if (currentQuestIdx >= questList.Count && questRewardShown)
        {
            isPreviousQuestCompleted = false;
        }
        if (nextQuestShown)
        {
            isPreviousQuestCompleted = false;
        }
        if (startedSavedGame)
        {
            isPreviousQuestCompleted = false;
        }

        // Start next quest
        if (isPreviousQuestCompleted && !questRewardShown)
        {
            ScoreManager.isTimePaused = true;
            CoinManager.coins += currentActiveQuest.coinReward;

            // Show quest rewards UI
            questRewardIndex.text = "Quest " + (currentQuestIdx + 1);
            questRewardTitle.text = currentActiveQuest.title;
            questRewardCoins.text = currentActiveQuest.coinReward.ToString();

            ShowQuestRewardUI();

            isPreviousQuestCompleted = false;
            currentQuestIdx += 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && questRewardShown)
        {
            HideQuestRewardUI();

            ShopManager.isShopAvailable = true;
            questRewardShown = false;
            nextQuestShown = true;

            if (currentQuestIdx >= questList.Count)
            {
                Debug.Log("Final boss is defeated!");
                SaveToScoreboard();

                RedirectToEnding();

                return;
            }

            currentActiveQuest = questList[currentQuestIdx];
            questCountdownIndex.text = "Quest " + (currentQuestIdx + 1) + " starting in:";

            timeLeftUntilNextQuest = 15f;
        }

        if (timeLeftUntilNextQuest <= 0f && nextQuestShown)
        {
            GenerateQuest();

            nextQuestShown = false;
            startedSavedGame = false;
        }
    }

    void GenerateQuest()
    {
        // Start timer score
        ScoreManager.isTimePaused = false;
        ShopManager.isShopAvailable = false;

        // Set enemies maximum spawn number, number of spawned, and number of killed
        currentZombearSpawnNumber = currentActiveQuest.zombearSpawnNumber;
        currentZombunnySpawnNumber = currentActiveQuest.zombunnySpawnNumber;
        currentHellephantSpawnNumber = currentActiveQuest.hellephantSpawnNumber;
        currentBossSpawnNumber = currentActiveQuest.bossSpawnNumber;
        currentZombearSpawned = 0;
        currentZombunnySpawned = 0;
        currentHellephantSpawned = 0;
        currentBossSpawned = 0;
        currentZombearKilled = 0;
        currentZombunnyKilled = 0;
        currentHellephantKilled = 0;
        currentBossKilled = 0;

        // Invoke every enemies
        InvokeZombear(
            currentActiveQuest.zombearIsSpawnAfterDeath,
            currentActiveQuest.zombearSpawnTime
        );

        InvokeZombunny(
            currentActiveQuest.zombunnyIsSpawnAfterDeath,
            currentActiveQuest.zombunnySpawnTime
        );

        InvokeHellephant(
            currentActiveQuest.hellephantIsSpawnAfterDeath,
            currentActiveQuest.hellephantSpawnTime
        );

        InvokeBoss(currentActiveQuest.bossIsSpawnAfterDeath, currentActiveQuest.bossSpawnTime);


    }

    bool questCompleted()
    {
        if (currentQuestIdx == 3)
        {
            return currentBossKilled > 0;
        }
        else
        {
            return (
            currentZombearKilled == currentZombearSpawnNumber &&
            currentZombunnyKilled == currentZombunnySpawnNumber &&
            currentHellephantKilled == currentHellephantSpawnNumber
        );
        }

    }

    public static void AddEnemyKilled(string enemyName)
    {
        if (enemyName == "Zombear")
        {
            currentZombearKilled += 1;
        }
        else if (enemyName == "Zombunny")
        {
            currentZombunnyKilled += 1;
        }
        else if (enemyName == "Hellephant")
        {
            currentHellephantKilled += 1;
        }
        else if (enemyName == "Boss")
        {
            currentBossKilled += 1;
        }
    }

    void ShowQuestRewardUI()
    {
        questRewardUI.SetActive(true);
        playerMovement.enabled = false;
        playerShooting.enabled = false;

        questRewardShown = true;
    }

    void HideQuestRewardUI()
    {
        questRewardUI.SetActive(false);
        playerMovement.enabled = true;
        playerShooting.enabled = true;

        questRewardShown = false;
    }

    void InvokeZombear(bool isSpawnAfterDeath, float spawnTime)
    {
        InvokeRepeating("SpawnZombear", 0f, spawnTime);
    }

    void InvokeZombunny(bool isSpawnAfterDeath, float spawnTime)
    {
        InvokeRepeating("SpawnZombunny", 0f, spawnTime);
    }

    void InvokeHellephant(bool isSpawnAfterDeath, float spawnTime)
    {
        InvokeRepeating("SpawnHellephant", 0f, spawnTime);
    }
    void InvokeBoss(bool isSpawnAfterDeath, float spawnTime)
    {
        Debug.Log("Invoking Boss");
        InvokeRepeating("SpawnBoss", 0f, spawnTime);
    }

    void SpawnZombear()
    {
        if (playerHealth.currentHealth <= 0f || currentZombearSpawned >= currentZombearSpawnNumber)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, zombearSpawnPoints.Length);
        Instantiate(zombear, zombearSpawnPoints[spawnPointIndex].position, zombearSpawnPoints[spawnPointIndex].rotation);

        currentZombearSpawned += 1;
    }

    void SpawnZombunny()
    {
        if (playerHealth.currentHealth <= 0f || currentZombunnySpawned >= currentZombunnySpawnNumber)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, zombunnySpawnPoints.Length);
        Instantiate(zombunny, zombunnySpawnPoints[spawnPointIndex].position, zombunnySpawnPoints[spawnPointIndex].rotation);

        currentZombunnySpawned += 1;
    }

    void SpawnHellephant()
    {
        if (playerHealth.currentHealth <= 0f || currentHellephantSpawned >= currentHellephantSpawnNumber)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, hellephantSpawnPoints.Length);
        Instantiate(hellephant, hellephantSpawnPoints[spawnPointIndex].position, hellephantSpawnPoints[spawnPointIndex].rotation);

        currentHellephantSpawned += 1;
    }

    void SpawnBoss()
    {
        if (playerHealth.currentHealth <= 0f || currentBossSpawned >= currentBossSpawnNumber)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, bossSpawnPoints.Length);
        Instantiate(Boss, bossSpawnPoints[spawnPointIndex].position, bossSpawnPoints[spawnPointIndex].rotation);
        currentBossSpawned += 1;


    }

    void SaveToScoreboard()
    {
        ScoreboardManager.AddScore(new Scoreboard(PlayerName.GetName(), ScoreManager.score));
        ScoreboardManager.SaveScoreboard();
    }

    void RedirectToEnding()
    {
        SceneManager.LoadScene("Ending_Scene", LoadSceneMode.Single);
    }

    public void onCloseButton()
    {
        if (currentQuestIdx == 3)
        {
            SaveManager.goToCutScene();
        }
        else
        {
            HideQuestRewardUI();

            ShopManager.isShopAvailable = true;
            questRewardShown = false;
            nextQuestShown = true;

            if (currentQuestIdx >= questList.Count)
            {
                Debug.Log("Final boss is defeated!");
                SaveToScoreboard();

                RedirectToEnding();

                return;
            }

            currentActiveQuest = questList[currentQuestIdx];
            questCountdownIndex.text = "Quest " + (currentQuestIdx + 1) + " starting in:";

            timeLeftUntilNextQuest = 15f;
        }
    }

    public void OnLoadCloseButton()
    {
        StartCoroutine(LoadSaveSceneAsync());
    }

    IEnumerator LoadSaveSceneAsync()
    {
        yield return null; // Wait one frame before loading the scene
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }
}
