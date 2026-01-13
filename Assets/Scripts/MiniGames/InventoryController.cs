using System.Text;
using System.Collections.Generic;
using UnityEngine;



public class InventoryController : MonoBehaviour
{
    public Dictionary<Item, int> items = new Dictionary<Item,int>();
    public Dictionary<Item, int> itemInOrder = new Dictionary<Item,int>();

    [SerializeField]private TMPro.TMP_Text inven;

    [SerializeField]
    private GameHUD gameHUD;

    private void Update()
    {
        inven.text = "Inventory: \n" + PrintDictionary();
    }

    private string PrintDictionary()
    {
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<Item, int> pair in items)
        {
            sb.Append(pair.Key).Append(": ").Append(pair.Value).Append("\n");
        }
        return sb.ToString();
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

    public string CreatOrder()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 3; i++)
        {
            int amount = Random.Range(1, 4);
            switch (Random.Range(0, 5))
            {
                case 0:
                    if (itemInOrder.ContainsKey(Item.RedApple))
                        break;
                    itemInOrder.Add(Item.RedApple, amount);
                    sb.Append(amount).Append(" x ").Append("Red Apples\n");
                    break;
                case 1:
                    if (itemInOrder.ContainsKey(Item.GreenApple))
                        break;
                    itemInOrder.Add(Item.GreenApple, amount);
                    sb.Append(amount).Append(" x ").Append("Green Apples\n");
                    break;
                case 2:
                    if (itemInOrder.ContainsKey(Item.Pear))
                        break;
                    itemInOrder.Add(Item.Pear, amount);
                    sb.Append(amount).Append(" x ").Append("Pears\n");
                    break;
                case 3:
                    if (itemInOrder.ContainsKey(Item.WhiteBread))
                        break;
                    itemInOrder.Add(Item.WhiteBread,amount);
                    sb.Append(amount).Append(" x ").Append("White Bread\n");
                    break;
                case 4:
                    if (itemInOrder.ContainsKey(Item.BlackBread))
                        break;
                    itemInOrder.Add(Item.BlackBread, amount);
                    sb.Append(amount).Append(" x ").Append("Black Bread\n");
                    break;
                case 5:
                    if (itemInOrder.ContainsKey(Item.BrownBread))
                        break;
                    itemInOrder.Add(Item.BrownBread,amount);
                    sb.Append(amount).Append(" x ").Append("Brown Bread\n");
                    break;
            }
        }
        return sb.ToString();
    }
    
    public bool CompleteOrder()
    {
        // 1. First, check if the inventory has enough of EVERY item in the order
        foreach (var orderPair in itemInOrder)
        {
            Item requiredItem = orderPair.Key;
            int requiredAmount = orderPair.Value;

            if (!items.ContainsKey(requiredItem) || items[requiredItem] < requiredAmount)
            {
                Debug.Log($"Missing items for order: Need {requiredAmount} {requiredItem}");
                return false; // Exit early because we can't fulfill the whole order
            }
        }

        // 2. If we reached this point, we have everything. Now subtract.
        foreach (var orderPair in itemInOrder)
        {
            items[orderPair.Key] -= orderPair.Value;
        }

        // 3. Clear the order so it's ready for a new one
        itemInOrder.Clear();
        gameHUD.order.text = CreatOrder();
        Debug.Log("Order Completed Successfully!");
        return true;
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
