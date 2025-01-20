using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    #region Events
    public event Action OnStatsUpgrade;
    public event Action OnTemporaryPowerUp;
    #endregion

    #region Stats
    private int currency = 0;
    public int Currency
    {
        get => currency;
    }

    [Header("Jump/Dash Mechanics")]
    public int initialDashes = 2;
    public float dashTime = 1f;
    public float timeSlowIntensity = 0.5f;

    [Header("Player Stats")]
    public int maxLives = 3;

    [Header("Coin and Luck")]
    public float coinCollectionRadius = 1f;
    public float luckMultiplier = 1f;

    [Header("Temporary Power Ups")]
    public float temporaryPowerUpDuration = 5f;

    [Header("Meta Power Ups")]
    public MetaPowerUpConfig dashUpgrade;
    public MetaPowerUpConfig dashTimeUpgrade;
    public MetaPowerUpConfig timeSlowUpgrade;
    public MetaPowerUpConfig coinCollectionUpgrade;
    public MetaPowerUpConfig luckUpgrade;
    public MetaPowerUpConfig tempPowerUpUpgrade;
    #endregion

    #region Upgrades
    public void UpgradeDash()
    {
        if (currency >= dashUpgrade.price && dashUpgrade.maxLevel > 0)
        {
            currency -= dashUpgrade.price;
            dashUpgrade.price = (int)(dashUpgrade.basePrice * Mathf.Pow(dashUpgrade.priceMultiplier, dashUpgrade.maxLevel - 1));
            dashUpgrade.level++;
            OnStatsUpgrade?.Invoke();
            initialDashes += (int)dashUpgrade.amountPerLevel;
        }
    }
    public void UpgradeDashTime()
    {
        if (currency >= dashTimeUpgrade.price && dashTimeUpgrade.maxLevel > 0)
        {
            currency -= dashTimeUpgrade.price;
            dashTimeUpgrade.price = (int)(dashTimeUpgrade.basePrice * Mathf.Pow(dashTimeUpgrade.priceMultiplier, dashTimeUpgrade.maxLevel - 1));
            dashTimeUpgrade.level++;
            OnStatsUpgrade?.Invoke();
            dashTime += dashTimeUpgrade.amountPerLevel;
        }
    }

    public void UpgradeTimeSlow()
    {
        if (currency >= timeSlowUpgrade.price && timeSlowUpgrade.maxLevel > 0)
        {
            currency -= timeSlowUpgrade.price;
            timeSlowUpgrade.price = (int)(timeSlowUpgrade.basePrice * Mathf.Pow(timeSlowUpgrade.priceMultiplier, timeSlowUpgrade.maxLevel - 1));
            timeSlowUpgrade.level++;
            OnStatsUpgrade?.Invoke();
            timeSlowIntensity += timeSlowUpgrade.amountPerLevel;
        }
    }

    public void UpgradeCoinCollection()
    {
        if (currency >= coinCollectionUpgrade.price && coinCollectionUpgrade.maxLevel > 0)
        {
            currency -= coinCollectionUpgrade.price;
            coinCollectionUpgrade.price = (int)(coinCollectionUpgrade.basePrice * Mathf.Pow(coinCollectionUpgrade.priceMultiplier, coinCollectionUpgrade.maxLevel - 1));
            coinCollectionUpgrade.level++;
            OnStatsUpgrade?.Invoke();
            coinCollectionRadius += coinCollectionUpgrade.amountPerLevel;
        }
    }

    public void UpgradeLuck()
    {
        if (currency >= luckUpgrade.price && luckUpgrade.maxLevel > 0)
        {
            currency -= luckUpgrade.price;
            luckUpgrade.price = (int)(luckUpgrade.basePrice * Mathf.Pow(luckUpgrade.priceMultiplier, luckUpgrade.maxLevel - 1));
            luckUpgrade.level++;
            OnStatsUpgrade?.Invoke();
            luckMultiplier += luckUpgrade.amountPerLevel;
        }
    }

    public void UpgradeTempPowerUp()
    {
        if (currency >= tempPowerUpUpgrade.price && tempPowerUpUpgrade.maxLevel > 0)
        {
            currency -= tempPowerUpUpgrade.price;
            tempPowerUpUpgrade.price = (int)(tempPowerUpUpgrade.basePrice * Mathf.Pow(tempPowerUpUpgrade.priceMultiplier, tempPowerUpUpgrade.maxLevel - 1));
            tempPowerUpUpgrade.level++;
            OnStatsUpgrade?.Invoke();
            temporaryPowerUpDuration += tempPowerUpUpgrade.amountPerLevel;
        }
    }
    #endregion

    public void AddCurrency(int amount)
    {
        currency += amount;
    }
}

[System.Serializable]
public class MetaPowerUpConfig
{
    public string powerUpName;
    public int basePrice = 10;
    public int price;
    public float priceMultiplier = 5f;
    public int maxLevel = 10;
    public int level;

    public float amountPerLevel;
}
