using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBoard : MonoBehaviour
{
    private MoneyDummy _moneyDummy;

    private void Awake()
    {
        _moneyDummy = GetComponentInChildren<MoneyDummy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetInitMoney();
        }
    }

    private void SetInitMoney()
    {
        _moneyDummy.AddMoneyObject(36);
    }
}
