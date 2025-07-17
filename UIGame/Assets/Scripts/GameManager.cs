using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private MarketSystem marketSystem;

    [Header("Resources")]
    [SerializeField] private int days = 1;
    [SerializeField] private int workingPopulation;
    [SerializeField] private int unemployedPopulation;
    [SerializeField] private int wood;
    [SerializeField] private int food;
    [SerializeField] private int stone;
    [SerializeField] private int iron;
    [SerializeField] private int gold;
    [SerializeField] private int tools;

    [Header("Buildings")]
    [SerializeField] private int houses;
    [SerializeField] private int lumberMills;
    [SerializeField] private int farms;
    [SerializeField] private int quarry;
    [SerializeField] private int ironMines;
    [SerializeField] private int goldMines;
    [SerializeField] private int forges;

    [Header("Resources Text")]
    [SerializeField] private TMP_Text daysText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text foodText;

    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text ironText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text toolsText;

    [Header("Buildings Text")]
    [SerializeField] private TMP_Text housesText;
    [SerializeField] private TMP_Text farmsText;
    [SerializeField] private TMP_Text lumberMillsText;
    [SerializeField] private TMP_Text quarryText;


    [Header("Advanced Buildings Text")]
    [SerializeField] private TMP_Text ironMinesText;
    [SerializeField] private TMP_Text goldMinesText;
    [SerializeField] private TMP_Text forgesText;


    [Header("Notifications")]
    [SerializeField] private TMP_Text notificationText;

    [Header("Unique Buildings")]
    [SerializeField] private bool hasMarket;
    [SerializeField] private GameObject useMarketButton;
    [SerializeField] private bool hasTemple;
    [SerializeField] private GameObject useTempleButton;
    [SerializeField] private TMP_Text theOfferingText;

    [Header("Upgrades")]
    [SerializeField] private bool hasBetterHouses;

    [Header("Win Panels")]
    [SerializeField] private GameObject winPanel1;
    [SerializeField] private GameObject winPanel2;
    [SerializeField] private GameObject losePanel;

    bool isGameRunning = false;

    private float timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //One minute is one day
        TimeOfDay();
    }

    private void Start()
    {
        UpdateText();
    }

    public void InitializeGame()
    {
        isGameRunning = true;
        UpdateText();
    }


    private void TimeOfDay()
    {
        if (!isGameRunning)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= 5) // Update every second
        {
            timer = 0f;
            days++;
            UpdateText();

            int foodAtStartOfDay = food;

            CalculateFood();
            CalculatePopulationGrowth();
            CalculateWood();
            CalculateStone();
            CalculateIron();
            CalculateGold();
            CalculateTools();

            // Check for food loss condition
            if (foodAtStartOfDay <= 0 && food <= 0)
            {
                isGameRunning = false;
                losePanel.SetActive(true);
                Debug.Log("Game Over: You ran out of food!");
                return;
            }

            if (marketSystem != null)
            {
                marketSystem.RefreshMarketUI();
                Debug.Log("Market UI refreshed");
            }

            marketSystem.RefreshMarketUI();
            Debug.Log("Market UI refreshed");
        }
    }

    // --- Population Methods ---
    private void CalculatePopulationGrowth()
    {
        int maxPopulation = GetMaxPopulation();
        if (Population() <= maxPopulation)
        {
            if (days % 2 == 0) // Every 2 days
            {

                if (GetMaxPopulation() <= Population() + houses)
                {
                    unemployedPopulation = GetMaxPopulation() - workingPopulation;
                }
                else
                {
                    unemployedPopulation += houses;
                }
            }


        }
    }
    private int Population()
    {
        return workingPopulation + unemployedPopulation;
    }

    private int GetMaxPopulation()
    {
        int maxPopulation = houses * 4;
        return maxPopulation;
    }


    private void AssignWorkers(int workersToAssign)
    {
        unemployedPopulation -= workersToAssign;
        workingPopulation += workersToAssign;
    }

    private bool CanAssignWorkers(int workersToAssign)
    {
        return unemployedPopulation >= workersToAssign;
    }

    public void BuildHouse()
    {
        if (wood >= (5 + houses * 2))
        {
            wood -= 5 + houses * 2;
            houses++;
            Debug.Log($"House built! Total houses: {houses}");
            UpdateText();
        }
        else
        {
            int woodNeeded = (5 + houses * 2) - wood;
            Debug.Log($"Need {woodNeeded}W");
            string text = $"Need {woodNeeded}W";
            ShowNotification(text);
        }
    }

    // --- Food Methods ---
    private void CalculateFood()
    {
        FoodGathering();
        FarmerFoodProduction();
        CalculateFoodConsumption(1);
    }

    private void FoodGathering()
    {
        food += unemployedPopulation / 2;
    }

    private void FarmerFoodProduction()
    {
        if (farms > 0)
        {
            food += farms * 4;
        }
    }

    private void CalculateFoodConsumption(int foodConsumed)
    {
        food -= foodConsumed * Population();
    }
    public void BuildFarm()
    {
        if (wood >= 5 && CanAssignWorkers(2))
        {
            wood -= 5;
            farms++;
            AssignWorkers(2);
            UpdateText();
        }
        else
        {
            Debug.Log($"Not enough resources, you need additional {10 - wood} wood ");
            string text = $"Need {10 - wood}W";
            ShowNotification(text);
        }
    }

    // --- Wood Methods ---
    private void CalculateWood()
    {
        WoodGathering();
    }

    private void WoodGathering()
    {
        if (lumberMills > 0)
        {
            wood += lumberMills * 2; // Each lumber mill produces 2 wood per day
        }
        else
        {
            wood += unemployedPopulation / 4; // Unemployed population gathers wood
        }
    }

    public void BuildLumberMill()
    {
        if (wood >= 5 && stone > 0 && CanAssignWorkers(1))
        {
            wood -= 5;
            stone--;
            lumberMills++;
            AssignWorkers(1);
            UpdateText();
        }
        else
        {
            Debug.Log($"Not enough resources or workers, you need additional {5 - wood} wood and 1 stone ");
            string text = $"Need {5 - wood}W, 1S, 1U";
            ShowNotification(text);
        }
    }

    // --- Stone Methods ---
    private void CalculateStone()
    {
        StoneProduction();
    }

    private void StoneProduction()
    {
        stone += quarry * 2;
    }
    public void BuildQuarry()
    {
        if (wood >= 5 && CanAssignWorkers(1))
        {
            wood -= 5;
            quarry++;
            AssignWorkers(1);
            UpdateText();
        }
        else
        {
            Debug.Log($"Not enough resources or workers, you need additional {5 - wood} wood ");
            string text = $"Need {5 - wood}W, 1U";
            ShowNotification(text);
        }
    }

    // --- Update Text Methods ---

    private void UpdateText()
    {
        // --- Food ---
        int foodGathered = unemployedPopulation / 2;
        int farmerFood = farms > 0 ? farms * 4 : 0;
        int totalFoodGain = foodGathered + farmerFood;
        int totalFoodConsumption = Population(); // 1 per person per day
        int netFood = totalFoodGain - totalFoodConsumption;

        string netFoodText = "";
        if (netFood != 0)
        {
            if (netFood > 0)
            {
                netFoodText = " +" + netFood.ToString();
            }
            else
            {
                netFoodText = " " + netFood.ToString();
            }
        }

        // --- Wood ---
        int woodGain = 0;
        if (lumberMills > 0)
        {
            woodGain = lumberMills * 2;
        }
        else
        {
            woodGain = unemployedPopulation / 4;
        }
        int woodNet = woodGain; // No consumption in base production
        string netWoodText = "";
        if (woodNet != 0)
        {
            if (woodNet > 0)
            {
                netWoodText = " +" + woodNet.ToString();
            }
            else
            {
                netWoodText = " " + woodNet.ToString();
            }
        }

        // --- Stone ---
        int stoneGain = quarry * 2;
        int stoneNet = stoneGain; // No consumption in base production
        string netStoneText = "";
        if (stoneNet != 0)
        {
            if (stoneNet > 0)
            {
                netStoneText = " +" + stoneNet.ToString();
            }
            else
            {
                netStoneText = " " + stoneNet.ToString();
            }
        }

        // --- Iron ---
        int ironGain = ironMines * 1;
        int ironConsumption = 0;
        if (forges > 0 && iron >= forges * 2)
        {
            ironConsumption = forges * 2;
        }
        int ironNet = ironGain - ironConsumption;
        string netIronText = "";
        if (ironNet != 0)
        {
            if (ironNet > 0)
            {
                netIronText = " +" + ironNet.ToString();
            }
            else
            {
                netIronText = " " + ironNet.ToString();
            }
        }

        // --- Gold ---
        int goldGain = goldMines * 1;
        int goldNet = goldGain; // No consumption in base production
        string netGoldText = "";
        if (goldNet != 0)
        {
            if (goldNet > 0)
            {
                netGoldText = " +" + goldNet.ToString();
            }
            else
            {
                netGoldText = " " + goldNet.ToString();
            }
        }

        // --- Tools ---
        int toolsGain = 0;
        if (forges > 0 && iron >= forges * 2)
        {
            toolsGain = forges * 1;
        }
        int toolsNet = toolsGain; // No consumption in base production
        string netToolsText = "";
        if (toolsNet != 0)
        {
            if (toolsNet > 0)
            {
                netToolsText = " +" + toolsNet.ToString();
            }
            else
            {
                netToolsText = " " + toolsNet.ToString();
            }
        }

        daysText.text = $"Days: {days}";
        populationText.text = $"Population: {Population()} / {GetMaxPopulation()} (Working: {workingPopulation}, Unemployed: {unemployedPopulation})";
        foodText.text = $"Food: {food}{netFoodText}";
        woodText.text = $"Wood: {wood}{netWoodText}";
        stoneText.text = $"Stone: {stone}{netStoneText}";
        ironText.text = $"Iron: {iron}{netIronText}";
        goldText.text = $"Gold: {gold}{netGoldText}";
        toolsText.text = $"Tools: {tools}{netToolsText}";

        housesText.text = $"Build House\n" +
            $"Cost: 2U {5 + houses * 2}W\n" +
            $"Houses: {houses}";
        farmsText.text = $"Build Farm\n" +
            $"Cost: 2U 5W\n" +
            $"Farms: {farms}";
        lumberMillsText.text = $"Build Lumber Mill\n" +
            $"Cost: 1U 5W 1S\n" +
            $"Lumber Mills: {lumberMills}";
        quarryText.text = $"Build Quarry\n" +
            $"Cost: 1U 5W\n" +
            $"Quarries: {quarry}";

        ironMinesText.text = $"Build Iron Mine\n" +
            $"Cost: 2U 10W 3S\n" +
            $"Iron Mines: {ironMines}";
        goldMinesText.text = $"Build Gold Mine\n" +
            $"Cost: 3U 8W 5S 2I\n" +
            $"Gold Mines: {goldMines}";
        forgesText.text = $"Build Forge\n" +
            $"Cost: 2U 15W 8S 5I\n" +
            $"Forges: {forges}";
    }

    // --- Notifications ---
    private Coroutine notificationCoroutine;

    public void ShowNotification(string message)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = message;
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = StartCoroutine(ClearNotificationCoroutine());
    }

    private IEnumerator ClearNotificationCoroutine()
    {
        yield return new WaitForSeconds(3f);
        notificationText.text = "";
        notificationCoroutine = null;
        notificationText.gameObject.SetActive(false);
    }

    // --- Iron Methods ---
    private void CalculateIron()
    {
        IronProduction();
    }

    private void IronProduction()
    {
        iron += ironMines * 1; // Each iron mine produces 1 iron per day
    }

    public void BuildIronMine()
    {
        if (wood >= 10 && stone >= 3 && CanAssignWorkers(2))
        {
            wood -= 10;
            stone -= 3;
            ironMines++;
            AssignWorkers(2);
            UpdateText();
        }
        else
        {
            string text = $"Need {Mathf.Max(0, 10 - wood)}W, {Mathf.Max(0, 3 - stone)}S, 2U";
            ShowNotification(text);
        }
    }

    // --- Gold Methods ---
    private void CalculateGold()
    {
        GoldProduction();
    }

    private void GoldProduction()
    {
        gold += goldMines * 1; // Each gold mine produces 1 gold per day
    }

    public void BuildGoldMine()
    {
        if (wood >= 8 && stone >= 5 && iron >= 2 && CanAssignWorkers(3))
        {
            wood -= 8;
            stone -= 5;
            iron -= 2;
            goldMines++;
            AssignWorkers(3);
            UpdateText();
        }
        else
        {
            string text = $"Need {Mathf.Max(0, 8 - wood)}W, {Mathf.Max(0, 5 - stone)}S, {Mathf.Max(0, 2 - iron)}I, 3U";
            ShowNotification(text);
        }
    }

    // --- Tools/Forge Methods ---
    private void CalculateTools()
    {
        ToolsProduction();
    }

    private void ToolsProduction()
    {
        if (forges > 0 && iron >= forges * 2) // Each forge needs 2 iron to produce 1 tool
        {
            iron -= forges * 2;
            tools += forges * 1;
        }
    }

    public void BuildForge()
    {
        if (wood >= 15 && stone >= 8 && iron >= 5 && CanAssignWorkers(2))
        {
            wood -= 15;
            stone -= 8;
            iron -= 5;
            forges++;
            AssignWorkers(2);
            UpdateText();
        }
        else
        {
            string text = $"Need {Mathf.Max(0, 15 - wood)}W, {Mathf.Max(0, 8 - stone)}S, {Mathf.Max(0, 5 - iron)}I, 2U";
            ShowNotification(text);
        }
    }


    // --- Unique Buildings Methods ---

    public void ShowMarketButton()
    {
        useMarketButton.SetActive(true);
    }

    public void ShowOfferingButton()
    {
        useTempleButton.SetActive(true);
    }

    public void GiveTheOffering()
    {
        if (gold >= 100 && unemployedPopulation >= 20)
        {
            gold -= 100;
            int ending = Random.Range(0, 1);
            if (ending == 0)
            {
                winPanel1.SetActive(true);
                Debug.Log("You have won the game!");
            }
            else
            {
                winPanel2.SetActive(true);
                Debug.Log("You have won the game!");
            }

        }
        else
        {
            ShowNotification("Offer more gold and people.");
        }
    }



    public void BuildMarket()
    {
        if (wood >= 50 && stone >= 30 && iron >= 20 && gold >= 10)
        {
            wood -= 50;
            stone -= 30;
            iron -= 20;
            gold -= 10;
            hasMarket = true;
            Debug.Log("Market built!");
            UpdateText();
            useMarketButton.SetActive(true);

        }
        else
        {
            string text = $"Need {Mathf.Max(0, 50 - wood)}W, {Mathf.Max(0, 30 - stone)}S, {Mathf.Max(0, 20 - iron)}I, {Mathf.Max(0, 10 - gold)}G";
            ShowNotification(text);
        }
    }


    // --- Resources Getters and Setters ---

    public int Food
    {
        get => food;
        set
        {
            food = value;
            UpdateText();
        }
    }
    public int Wood
    {
        get => wood;
        set
        {
            wood = value;
            UpdateText();
        }
    }
    public int Stone
    {
        get => stone;
        set
        {
            stone = value;
            UpdateText();
        }
    }
    public int Iron
    {
        get => iron;
        set
        {
            iron = value;
            UpdateText();
        }
    }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            UpdateText();
        }
    }
    public int Tools
    {
        get => tools;
        set
        {
            tools = value;
            UpdateText();
        }
    }
    public int Days
    {
        get => days;
        set
        {
            days = value;
            UpdateText();
        }
    }
    public int WorkingPopulation
    {
        get => workingPopulation;
        set
        {
            workingPopulation = value;
            UpdateText();
        }
    }

    public bool HasMarket
    {
        get => hasMarket;
        set
        {
            hasMarket = value;
        }
    }
    public int UnemployedPopulation
    {
        get => unemployedPopulation;
        set
        {
            unemployedPopulation = value;
            UpdateText();
        }
    }
    public bool HasTemple
    {
        get => hasTemple;
        set
        {
            hasTemple = value;
            UpdateText();
        }
    }




}


