
using System;
using System.Collections.Generic;
using UnityEngine;



public class InventoryController : MonoBehaviour
{
    public Dictionary<Item, int> items = new Dictionary<Item,int>();

    [SerializeField]private TMPro.TMP_Text inven;

    private void Update()
    {

        inven.text = "Inventory: " + PrintDictionary();
    }

    private string PrintDictionary()
    {
        string result = "";
        foreach (KeyValuePair<Item, int> pair in items)
        {
            result = pair.Key.ToString() + ": " + pair.Value.ToString()+ "/n";
        }
        return result;
    }

    public void AddItem(Item item)
    {
        
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items.Add(item, 1);
        }
    }

    public void RemoveItem(int amount, Item item)
    {
        if (items.ContainsKey(item))
        {
            // 2. Check if we have enough items (Value won't go below 0)
            if (items[item] >= amount)
            {
                items[item] -= amount;
            }
            else
            {
                // 3. Not enough items in the value to subtract
                Debug.Log("Insufficient items");
            }
        }
        else
        {
            // 4. Item isn't in the dictionary at all
            Debug.Log("Insufficient items");
        }
    }
}
