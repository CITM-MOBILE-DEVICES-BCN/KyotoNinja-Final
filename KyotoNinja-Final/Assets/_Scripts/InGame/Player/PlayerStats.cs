using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [HideInInspector]
    public string filePath;

    #region Events
    public event Action OnStatsUpgrade;
    public event Action OnTemporaryPowerUp;
    #endregion

    #region Stats
    [Header("Currency")]
    public int currency = 0;

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
    #endregion

    #region Upgrades
    public void UpgradeMetaPowerUp(string name)
    {
        foreach (var metaPowerUp in metaPowerUps)
        {
            if (metaPowerUp.powerUpName == name)
            {
                if (currency >= metaPowerUp.price && metaPowerUp.maxLevel > 0 && metaPowerUp.level < metaPowerUp.maxLevel)
                {
                    currency -= metaPowerUp.price;
                    metaPowerUp.price = (int)(metaPowerUp.basePrice * metaPowerUp.priceMultiplier);
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

        SaveToBinary(filePath);
    }
    #endregion

    public void AddCurrency(int amount)
    {
        currency += amount;

        SaveToBinary(filePath);
    }

    public void RefreshPrices()
    {
        foreach (var metaPowerUp in metaPowerUps)
        {
            if(metaPowerUp.level == 0)
            {
                metaPowerUp.price = metaPowerUp.basePrice;
            }
            else
            {
                metaPowerUp.price = (int)(metaPowerUp.basePrice + metaPowerUp.priceMultiplier * metaPowerUp.level);
            }
        }
    }

    public void ResetStats()
    {
        currency = 0;
        initialDashes = 1;
        dashTime = 0.5f;
        timeSlowIntensity = 0.5f;
        maxLives = 3;
        coinCollectionRadius = 1f;
        luckMultiplier = 1f;
        temporaryPowerUpDuration = 5f;

        foreach (var metaPowerUp in metaPowerUps)
        {
            metaPowerUp.level = 0;
            metaPowerUp.price = metaPowerUp.basePrice;
        }
    }

    public void CalculateStats()
    {
        foreach(var metaPowerUp in metaPowerUps)
        {
            switch(metaPowerUp.powerUpName)
            {
                case "Extra Dash":
                    initialDashes = (int)metaPowerUp.baseAmount + (int)metaPowerUp.amountPerLevel * metaPowerUp.level;
                    break;
                case "Dash Time":
                    dashTime = metaPowerUp.baseAmount + metaPowerUp.amountPerLevel * metaPowerUp.level;
                    break;
                case "Time-Slow":
                    timeSlowIntensity = metaPowerUp.baseAmount + metaPowerUp.amountPerLevel * metaPowerUp.level;
                    break;
                case "Collection Range":
                    coinCollectionRadius = metaPowerUp.baseAmount + metaPowerUp.amountPerLevel * metaPowerUp.level;
                    break;
                case "Luck":
                    luckMultiplier = metaPowerUp.baseAmount + metaPowerUp.amountPerLevel * metaPowerUp.level;
                    break;
                case "PowerUp Duration":
                    temporaryPowerUpDuration = metaPowerUp.baseAmount + metaPowerUp.amountPerLevel * metaPowerUp.level;
                    break;
            }
        }
    }

    public void SaveToBinary(string filePath)
    {
        PlayerSerializableData data = new PlayerSerializableData
        {
            currency = currency,
            metaPowerUps = metaPowerUps
        };

        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, data);
        fileStream.Close();
    }
}

[Serializable]
public class MetaPowerUpConfig
{
    public string powerUpName;
    public int basePrice = 2;
    public int price;
    public float priceMultiplier = 5f;
    public int maxLevel = 10;
    public int level = 1;
    public float amountPerLevel;
    public float baseAmount;
    public Sprite icon;
}

[Serializable]
public class PlayerSerializableData
{
    public int currency;
    public List<MetaPowerUpConfig> metaPowerUps;
}
