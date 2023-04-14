using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    GameObject player;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool started = false;

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
    public Transform[] zombearSpawnPoints;
    public Transform[] zombunnySpawnPoints;
    public Transform[] hellephantSpawnPoints;

    int currentZombearSpawnNumber;
    int currentZombunnySpawnNumber;
    int currentHellephantSpawnNumber;
    int currentZombearSpawned;
    int currentZombunnySpawned;
    int currentHellephantSpawned;
    public static int currentZombearKilled;
    public static int currentZombunnyKilled;
    public static int currentHellephantKilled;

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
    public Button closeButton;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerShooting = player.GetComponentInChildren<PlayerShooting>();
    }

    public void StartQuest()
    {
        started = true;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            HideQuestRewardUI();

            ShopManager.isShopAvailable = true;
            questRewardShown = false;
            nextQuestShown = true;

            if (currentQuestIdx >= questList.Count)
            {
                Debug.Log("Final boss is defeated!");
                return;
            }

            currentActiveQuest = questList[currentQuestIdx];
            questCountdownIndex.text = "Quest " + (currentQuestIdx + 1) + " starting in:";

            timeLeftUntilNextQuest = 15f;
        });

        // Start first quest
        currentActiveQuest = questList[currentQuestIdx];

        // Show quest UI
        // If key is clicked, start next quest
        GenerateQuest();
    }

    void Update()
    {
        if (!started)
        {
            return;
        }
        // Debug.Log(currentActiveQuest);
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
        currentZombearSpawned = 0;
        currentZombunnySpawned = 0;
        currentHellephantSpawned = 0;
        currentZombearKilled = 0;
        currentZombunnyKilled = 0;
        currentHellephantKilled = 0;

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
    }

    bool questCompleted()
    {
        return (
            currentZombearKilled == currentZombearSpawnNumber &&
            currentZombunnyKilled == currentZombunnySpawnNumber &&
            currentHellephantKilled == currentHellephantSpawnNumber
        );
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
}
