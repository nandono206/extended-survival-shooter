using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemTypeText;
    [SerializeField] TMP_Text itemPriceText;
    [SerializeField] Button itemPurchaseButton;
    
    public void SetItemPosition(Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition += position;
    }
    
    public void SetItemImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
    
    public void SetItemName(string name)
    {
        itemNameText.text = name;
    }
    
    public void SetItemType(string type)
    {
        itemTypeText.text = type;
    }
    
    public void SetItemPrice(int price)
    {
        itemPriceText.text = price.ToString();
    }
    
    public void SetItemAsPurchased()
    {
        itemPurchaseButton.gameObject.SetActive(false);
    }
    
    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        itemPurchaseButton.onClick.RemoveAllListeners();
        itemPurchaseButton.onClick.AddListener(() =>
        {
            action.Invoke(itemIndex);
        });
    }
}
