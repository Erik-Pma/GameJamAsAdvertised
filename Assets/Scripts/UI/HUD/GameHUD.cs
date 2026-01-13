using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField]TMPro.TMP_Text money;
    private int _moneyAmount = 0;
    [SerializeField]TMPro.TMP_Text order;
    
    

    private void Start()
    {
        money.text = "Money: " + _moneyAmount;
        order.text = "Order: " + GetOrder();
    }

    private String GetOrder()
    {
        return "this is a test";
    }
}
