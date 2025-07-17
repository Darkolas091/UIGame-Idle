using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketSystem : MonoBehaviour
{
    [Header("Trading Buttons")]
    [SerializeField] private Button buyFoodButton;
    [SerializeField] private Button sellFoodButton;
    [SerializeField] private Button buyWoodButton;
    [SerializeField] private Button sellWoodButton;
    [SerializeField] private Button buyStoneButton;
    [SerializeField] private Button sellStoneButton;
    [SerializeField] private Button buyIronButton;
    [SerializeField] private Button sellIronButton;

    [Header("Trading Text")]
    [SerializeField] private TMP_Text tradingInfoText;

    [SerializeField] private bool isMarketOpen = false;

    [SerializeField] private GameManager gameManager;

    // Fluctuating trading rates (gold per resource)
    private int woodPrice = 2;
    private int stonePrice = 4;
    private int ironPrice = 10;
    private int foodPrice = 2;

    // Price fluctuation settings
    private int minWoodPrice = 2, maxWoodPrice = 5;
    private int minStonePrice = 3, maxStonePrice = 7;
    private int minIronPrice = 8, maxIronPrice = 15;
    private int minFoodPrice = 2, maxFoodPrice = 4;

    private void Start()
    {
        gameManager = GameManager.Instance;
        FluctuatePrices();
        UpdateMarketUI();
    }

    // We call this every day
    public void RefreshMarketUI()
    {
        FluctuatePrices();
        UpdateMarketUI();
    }

    private void FluctuatePrices()
    {
        woodPrice = Random.Range(minWoodPrice, maxWoodPrice + 1);
        stonePrice = Random.Range(minStonePrice, maxStonePrice + 1);
        ironPrice = Random.Range(minIronPrice, maxIronPrice + 1);
        foodPrice = Random.Range(minFoodPrice, maxFoodPrice + 1);
    }

    private void UpdateMarketUI()
    {
        tradingInfoText.text = $"Trading Rates:\n" +
            $"Wood: {woodPrice} gold each\n" +
            $"Stone: {stonePrice} gold each\n" +
            $"Iron: {ironPrice} gold each\n" +
            $"Food: {foodPrice} gold each";
    }

    public void SellWood1() => SellResource("wood", 1);
    public void BuyWood1() => BuyResource("wood", 1);
    public void SellStone1() => SellResource("stone", 1);
    public void BuyStone1() => BuyResource("stone", 1);
    public void SellIron1() => SellResource("iron", 1);
    public void BuyIron1() => BuyResource("iron", 1);
    public void SellFood1() => SellResource("food", 1);
    public void BuyFood1() => BuyResource("food", 1);

    private void SellResource(string resourceType, int amount)
    {
        switch (resourceType)
        {
            case "wood":
                if (gameManager.Wood >= amount)
                {
                    gameManager.Wood -= amount;
                    gameManager.Gold += amount * woodPrice;
                    gameManager.ShowNotification($"Sold {amount} wood for {amount * woodPrice} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough wood! Need {amount}");
                }
                break;

            case "stone":
                if (gameManager.Stone >= amount)
                {
                    gameManager.Stone -= amount;
                    gameManager.Gold += amount * stonePrice;
                    gameManager.ShowNotification($"Sold {amount} stone for {amount * stonePrice} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough stone! Need {amount}");
                }
                break;

            case "iron":
                if (gameManager.Iron >= amount)
                {
                    gameManager.Iron -= amount;
                    gameManager.Gold += amount * ironPrice;
                    gameManager.ShowNotification($"Sold {amount} iron for {amount * ironPrice} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough iron! Need {amount}");
                }
                break;

            case "food":
                if (gameManager.Food >= amount)
                {
                    gameManager.Food -= amount;
                    gameManager.Gold += amount * foodPrice;
                    gameManager.ShowNotification($"Sold {amount} food for {amount * foodPrice} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough food! Need {amount}");
                }
                break;
        }
    }

    private void BuyResource(string resourceType, int amount)
    {
        int totalCost = 0;

        switch (resourceType)
        {
            case "wood":
                totalCost = amount * woodPrice;
                if (gameManager.Gold >= totalCost)
                {
                    gameManager.Gold -= totalCost;
                    gameManager.Wood += amount;
                    gameManager.ShowNotification($"Bought {amount} wood for {totalCost} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough gold! Need {totalCost}");
                }
                break;

            case "stone":
                totalCost = amount * stonePrice;
                if (gameManager.Gold >= totalCost)
                {
                    gameManager.Gold -= totalCost;
                    gameManager.Stone += amount;
                    gameManager.ShowNotification($"Bought {amount} stone for {totalCost} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough gold! Need {totalCost}");
                }
                break;

            case "iron":
                totalCost = amount * ironPrice;
                if (gameManager.Gold >= totalCost)
                {
                    gameManager.Gold -= totalCost;
                    gameManager.Iron += amount;
                    gameManager.ShowNotification($"Bought {amount} iron for {totalCost} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough gold! Need {totalCost}");
                }
                break;

            case "food":
                totalCost = amount * foodPrice;
                if (gameManager.Gold >= totalCost)
                {
                    gameManager.Gold -= totalCost;
                    gameManager.Food += amount;
                    gameManager.ShowNotification($"Bought {amount} food for {totalCost} gold");
                }
                else
                {
                    gameManager.ShowNotification($"Not enough gold! Need {totalCost}");
                }
                break;
        }
    }

    public bool IsMarketOpen()
    {
        return isMarketOpen;
    }

    public void SetMarketOpen(bool value)
    {
        isMarketOpen = value;
    }
}
