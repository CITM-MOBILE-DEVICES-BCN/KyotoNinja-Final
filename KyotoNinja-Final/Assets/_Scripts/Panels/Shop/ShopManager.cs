using MyNavigationSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private TMP_Text currencyText;

    [Header("Buttons")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button purchaseButton1;
    [SerializeField] private Button purchaseButton2;
    [SerializeField] private Button purchaseButton3;
    [SerializeField] private Button purchaseButton4;
    [SerializeField] private Button purchaseButton5;

    [Header("Prices")]
    [SerializeField] private TMP_Text dashPrice;
    [SerializeField] private TMP_Text dashTimePrice;
    [SerializeField] private TMP_Text timeStopPrice;
    [SerializeField] private TMP_Text coinCollectionPrice;
    [SerializeField] private TMP_Text luckPrice;

    [Header("PowerUpLevels")]
    [SerializeField] private TMP_Text dashLevel;
    [SerializeField] private TMP_Text dashTimeLevel;
    [SerializeField] private TMP_Text timeStopLevel;
    [SerializeField] private TMP_Text coinCollectionLevel;
    [SerializeField] private TMP_Text luckLevel;

    [Header("Button Actions")]
    [SerializeField] private string mainMenuSceneId;
    [SerializeField] private int stageTimeStop;    
    
    [Header("ShopIcon")]
    [SerializeField] private Image shopImage;


    void Start()
    {
        List<GameObject> images = new List<GameObject>
        {
            shopImage.gameObject
        };

        NavigationManager.Instance.StartAnim(images, 2);

        mainMenuButton.onClick.AddListener(() => NavigationManager.Instance.LoadSceneAsync(mainMenuSceneId));

        playerStats.RefreshPrices();

        purchaseButton1.onClick.AddListener(() => BuyUpgrade("Extra Dash"));
        purchaseButton2.onClick.AddListener(() => BuyUpgrade("Dash Time"));
        purchaseButton3.onClick.AddListener(() => BuyUpgrade("Time-Slow"));
        purchaseButton4.onClick.AddListener(() => BuyUpgrade("Collection Range"));
        purchaseButton5.onClick.AddListener(() => BuyUpgrade("Luck"));
    }

    private void Update()
    {
        currencyText.text = "Currency: " + playerStats.currency + "+";

        dashLevel.text = "Level " + (playerStats.metaPowerUps[0].level + 1);
        dashTimeLevel.text = "Level " + (playerStats.metaPowerUps[1].level + 1);
        timeStopLevel.text = "Level " + (playerStats.metaPowerUps[2].level + 1);
        coinCollectionLevel.text = "Level " + (playerStats.metaPowerUps[3].level + 1);
        luckLevel.text = "Level " + (playerStats.metaPowerUps[4].level + 1);

        dashPrice.text = "Upgrade " + playerStats.metaPowerUps[0].price + "+";
        dashTimePrice.text = "Upgrade " + playerStats.metaPowerUps[1].price + "+";
        timeStopPrice.text = "Upgrade " + playerStats.metaPowerUps[2].price + "+";
        coinCollectionPrice.text = "Upgrade " + playerStats.metaPowerUps[3].price + "+";
        luckPrice.text = "Upgrade " + playerStats.metaPowerUps[4].price + "+";
    }

    void BuyUpgrade(string name)
    {
        playerStats.UpgradeMetaPowerUp(name);
    }


}
