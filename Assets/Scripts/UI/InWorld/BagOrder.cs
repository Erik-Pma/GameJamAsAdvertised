using UI.InWorld;
using UnityEngine;

public class BagOrder : Interactable
{
    [SerializeField]private InventoryController inventory;
    [SerializeField] private GameHUD gameHUD;
    protected override void Interact()
    {
        if (interationAmt)
        {
            if (inventory.CompleteOrder())
            {
                gameHUD.moneyAmount += Random.Range(5, 20);
                gameHUD.money.text = "Money: " + gameHUD.moneyAmount;
            }
        }
    }
}
