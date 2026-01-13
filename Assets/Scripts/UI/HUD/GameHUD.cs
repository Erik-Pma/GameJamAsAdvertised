using UnityEngine;

public class GameHUD : MonoBehaviour
{
    public TMPro.TMP_Text money;
    public int moneyAmount = 0;
    public TMPro.TMP_Text order;
    [SerializeField]private InventoryController inventory;
    
    

    private void Start()
    {
        money.text = "Money: " + moneyAmount;
        order.text = "Order: " + GetOrder();
    }

    private string GetOrder()
    {
        return inventory.CreatOrder();
    }
}
