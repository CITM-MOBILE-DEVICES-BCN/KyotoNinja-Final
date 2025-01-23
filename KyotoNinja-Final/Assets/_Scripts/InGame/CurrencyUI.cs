using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public PlayerStats playerStats;
    private TMP_Text currencyText;

    private void Start()
    {
        currencyText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        currencyText.text = playerStats.currency.ToString();
    }
}
