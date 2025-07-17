    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
using UnityEngine.UI;

public class UniqueBuildingSystem : MonoBehaviour
{
    [Header("Building Buttons")]
    [SerializeField] private Button buildMarketButton;
    [SerializeField] private Button buildTempleButton;

    [Header("Building Info Text")]
    [SerializeField] private TMP_Text marketInfoText;
    [SerializeField] private TMP_Text templeInfoText;

    [Header("Button Texts")]
    [SerializeField] private TMP_Text buildMarketButtonText;
    [SerializeField] private TMP_Text buildTempleButtonText;

    [Header("Market Description")]
    [SerializeField] private TMP_Text marketDescriptionText;

    [Header("Temple Description")]
    [SerializeField] private TMP_Text templeDescriptionText;

    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        UpdateMarketButtonText();
        UpdateMarketDescription();
        UpdateMarketButtonInteractable();

        UpdateTempleButtonText();
        UpdateTempleDescription();
        UpdateTempleButtonInteractable();
    }

    public void RefreshUniqueBuildingsUI()
    {
        UpdateUniqueBuildingsUI();
    }

    private void UpdateUniqueBuildingsUI()
    {
        UpdateMarketButtonText();
        UpdateMarketDescription();
        UpdateMarketButtonInteractable();

        UpdateTempleButtonText();
        UpdateTempleDescription();
        UpdateTempleButtonInteractable();
    }

    private void UpdateMarketButtonText()
    {
        if (gameManager != null && gameManager.HasMarket)
        {
            buildMarketButtonText.text = "Market: BUILT";
            marketInfoText.text = "Market: BUILT";
        }
        else
        {
            buildMarketButtonText.text = "Market (50W 30S 20I 10G)";
            marketInfoText.text = "Market: NOT BUILT (50W 30S 20I 10G)";
        }
    }

    private void UpdateMarketDescription()
    {
        if (marketDescriptionText != null)
        {
            marketDescriptionText.text = "Enables resource trading";
        }
    }

    private void UpdateMarketButtonInteractable()
    {
        if (gameManager != null && gameManager.HasMarket)
        {
            buildMarketButton.interactable = false;
        }
        else
        {
            buildMarketButton.interactable = CanAffordMarket();
        }
    }

    private bool CanAffordMarket()
    {
        return gameManager.Wood >= 50 && gameManager.Stone >= 30 &&
               gameManager.Iron >= 20 && gameManager.Gold >= 10;
    }

    public void BuildMarket()
    {
        if (!gameManager.HasMarket && CanAffordMarket())
        {
            gameManager.BuildMarket();
            UpdateUniqueBuildingsUI();
            gameManager.ShowMarketButton();
            Debug.Log("Market built successfully!");
        }
    }

    // --- Temple Building ---

    private void UpdateTempleButtonText()
    {
        if (gameManager != null && gameManager.HasTemple)
        {
            buildTempleButtonText.text = "Temple: BUILT";
            templeInfoText.text = "Temple: BUILT";
        }
        else
        {
            buildTempleButtonText.text = "Temple (50W 30S 20I 10G)";
            templeInfoText.text = "Temple: NOT BUILT (50W 30S 20I 10G)";
        }
    }

    private void UpdateTempleDescription()
    {
        if (templeDescriptionText != null)
        {
            templeDescriptionText.text = "Do you dare to offer to the gods?";
        }
    }

    private void UpdateTempleButtonInteractable()
    {
        if (gameManager != null && gameManager.HasTemple)
        {
            buildTempleButton.interactable = false;
        }
        else
        {
            buildTempleButton.interactable = CanAffordTemple();
        }
    }

    private bool CanAffordTemple()
    {
        return gameManager.Wood >= 50 && gameManager.Stone >= 30 &&
               gameManager.Iron >= 20 && gameManager.Gold >= 10;
    }

    public void BuildTemple()
    {
        if (!gameManager.HasTemple && CanAffordTemple())
        {
            gameManager.Wood -= 50;
            gameManager.Stone -= 30;
            gameManager.Iron -= 20;
            gameManager.Gold -= 10;
            gameManager.HasTemple = true;
            gameManager.ShowOfferingButton();
            UpdateUniqueBuildingsUI();
            Debug.Log("Temple built successfully!");
        }
    }
}
