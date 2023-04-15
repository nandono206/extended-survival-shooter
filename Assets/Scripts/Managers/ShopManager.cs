using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Elements")]
    [SerializeField] Transform shopMenu;
    [SerializeField] Transform shopItemsContainer;
    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] ShopItemDatabase shopItemDatabase;
    [SerializeField] float itemSpacing = .5f;

    [Header("Shop Open/Close Events")]
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject shopCue;
    [SerializeField] Button closeButton;
    [SerializeField] Button shopButton;

    GameObject player;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    GameObject spawnerObj;
    Spawner spawner;
    bool playerInRange;
    bool shopShown = false;
    float itemWidth;

    public static bool isShopAvailable = true;

    void Awake()
    {
        // Peroleh game object dengan tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerShooting = player.GetComponentInChildren<PlayerShooting>();

        spawnerObj = GameObject.FindGameObjectWithTag("Spawner");
        spawner = spawnerObj.GetComponent<Spawner>();
    }

    void Start()
    {
        GenerateShopItemsUI();

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            shopShown = false;
            HideShopCue();
            HideShopPanel();
        });
        shopButton.onClick.RemoveAllListeners();
        shopButton.onClick.AddListener(() =>
        {
            shopShown = true;
            HideShopCue();
            ShowShopPanel();
        });
    }

    void GenerateShopItemsUI()
    {
        itemWidth = shopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        Destroy(shopItemsContainer.GetChild(0).gameObject);
        shopItemsContainer.DetachChildren();

        for (int i = 0; i < shopItemDatabase.ShopItemCount; i++)
        {
            ShopItem item = shopItemDatabase.GetShopItem(i);
            ShopItemUI uiItem = Instantiate(shopItemPrefab, shopItemsContainer).GetComponent<ShopItemUI>();

            uiItem.gameObject.name = item.name;

            uiItem.SetItemPosition(Vector2.right * i * (itemWidth + itemSpacing));
            uiItem.SetItemImage(item.image);
            uiItem.SetItemName(item.name);
            uiItem.SetItemType(item.type);
            uiItem.SetItemPrice(item.price);

            if (item.isPurchased)
            {
                uiItem.SetItemAsPurchased();
                SetAvailableItem(item.name);
            }
            else
            {
                uiItem.SetItemPrice(item.price);
                uiItem.OnItemPurchase(i, OnItemPurchased);
            }
            if (item.name == "Shotgun" && PlayerShooting.isShotgunAvailable)
            {
                uiItem.SetItemAsPurchased();
                SetAvailableItem(item.name);
            }
            else if (item.name == "Bow" && PlayerShooting.isBowAvailable)
            {
                uiItem.SetItemAsPurchased();
                SetAvailableItem(item.name);
            }
            else if (item.name == "Sword" && PlayerShooting.isSwordAvailable)
            {
                uiItem.SetItemAsPurchased();
                SetAvailableItem(item.name);
            }

            shopItemsContainer.GetComponent<RectTransform>().sizeDelta = Vector2.left * ((itemWidth + itemSpacing) * shopItemDatabase.ShopItemCount + itemSpacing);
        }
    }

    void OnItemPurchased(int index)
    {
        ShopItem item = shopItemDatabase.GetShopItem(index);
        ShopItemUI uiItem = GetShopItemUI(index);

        if (CanPurchase(item.price))
        {
            // Purchase item
            CoinManager.coins -= item.price;
            uiItem.SetItemAsPurchased();

            SetAvailableItem(item.name);
        }
        else
        {
            uiItem.SetItemPrice(item.price);
            uiItem.OnItemPurchase(index, OnItemPurchased);
        }
    }

    ShopItemUI GetShopItemUI(int index)
    {
        return shopItemsContainer.GetChild(index).GetComponent<ShopItemUI>();
    }

    bool CanPurchase(int price)
    {
        return (price <= CoinManager.coins);
    }

    void SetAvailableItem(string name)
    {
        if (name == "Shotgun")
        {
            PlayerShooting.isShotgunAvailable = true;
        }
        else if (name == "Bow")
        {
            PlayerShooting.isBowAvailable = true;
        }
        else if (name == "Sword")
        {
            PlayerShooting.isSwordAvailable = true;
        }
        else if (name == "Fox")
        {
            spawner.SpawnObject(0);
        }
        else if (name == "Dragon")
        {
            spawner.SpawnObject(1);
        }
        else if (name == "Bear")
        {
            spawner.SpawnObject(2);
        }
    }

    void Update()
    {
        // Jika playerInRange, buka toko
        if (playerInRange && playerHealth.currentHealth > 0 && isShopAvailable)
        {
            if (!shopShown)
            {
                ShowShopCue();
            }

            if (Input.GetKeyDown(KeyCode.X) && !shopShown)
            {
                shopShown = true;
                HideShopCue();
                ShowShopPanel();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && shopShown)
            {
                shopShown = false;
                HideShopCue();
                HideShopPanel();
            }
        }
        else
        {
            HideShopCue();
            HideShopPanel();
            shopShown = false;
        }
    }

    // Show shop UI
    void ShowShopPanel()
    {
        shopPanel.SetActive(true);
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    // Hide shop UI
    void HideShopPanel()
    {
        shopPanel.SetActive(false);
        playerMovement.enabled = true;
        playerShooting.enabled = true;
    }

    // Show shop Cue
    void ShowShopCue()
    {
        shopCue.SetActive(true);
    }

    // Hide shop Cue
    void HideShopCue()
    {
        shopCue.SetActive(false);
    }

    // Jika sesuatu collide dengan enemy
    void OnTriggerEnter(Collider other)
    {
        // Jika player, playerinrange true
        if (other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = true;
        }
    }

    // Kebalikan OnTriggerEnter
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }
}
