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
    public List<MetaPowerUpConfig> metaPowerUps = new List<MetaPowerUpConfig>();
    //public MetaPowerUpConfig dashUpgrade;
    //public MetaPowerUpConfig dashTimeUpgrade;
    //public MetaPowerUpConfig timeSlowUpgrade;
    //public MetaPowerUpConfig coinCollectionUpgrade;
    //public MetaPowerUpConfig luckUpgrade;
    //public MetaPowerUpConfig tempPowerUpUpgrade;
    #endregion

    #region Upgrades
    public void UpgradeMetaPowerUp(string name)
    {
        foreach(var metaPowerUp in metaPowerUps)
        {
            if (metaPowerUp.powerUpName == name)
            {
                if (currency >= metaPowerUp.price && metaPowerUp.maxLevel > 0)
                {
                    currency -= metaPowerUp.price;
                    metaPowerUp.price = (int)(metaPowerUp.basePrice * Mathf.Pow(metaPowerUp.priceMultiplier, metaPowerUp.maxLevel - 1));
                    metaPowerUp.level++;
                    OnStatsUpgrade?.Invoke();
                    switch (name)
                    {
                        case "Extra Dash":
                            initialDashes += (int)metaPowerUp.amountPerLevel;
                            break;
                        case "Dash Time":
                            dashTime += metaPowerUp.amountPerLevel;
                            break;
                        case "Time-Slow":
                            timeSlowIntensity += metaPowerUp.amountPerLevel;
                            break;
                        case "Collection Range":
                            coinCollectionRadius += metaPowerUp.amountPerLevel;
                            break;
                        case "Luck":
                            luckMultiplier += metaPowerUp.amountPerLevel;
                            break;
                        case "PowerUp Duration":
                            temporaryPowerUpDuration += metaPowerUp.amountPerLevel;
                            break;
                    }
                }
            }



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
    public int basePrice = 2;
    public int price;
    public float priceMultiplier = 5f;
    public int maxLevel = 10;
    public int level;

    public float amountPerLevel;
}
