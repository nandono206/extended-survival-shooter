using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ShopItemDatabase", menuName = "Shop/ShopItemDatabase")]
public class ShopItemDatabase : ScriptableObject
{
    public ShopItem[] shopItems;
   
    public int ShopItemCount {
        get
        {
            return shopItems.Length;
        }
    }
    
    public ShopItem GetShopItem(int index) {
        return shopItems[index];
    }
    
    public void PurchaseShopItem(int index) {
        shopItems[index].isPurchased = true;
    }
}
