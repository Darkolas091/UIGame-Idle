using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string HIGH_SCORE_NAME_KEY = "HighScoreName";
    private int bestDaysSurvived = 0;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private MarketSystem marketSystem;

    [Header("Main Panels")]
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("Game Panels")] 
    [SerializeField] private GameObject marketPanel;
    [SerializeField] private GameObject uniqueBuildingsPanel;

    //Save Button
    [SerializeField] private Button saveButton;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text highScoreText;

    [Header("Systems")]
    //[SerializeField] private MarketSystem marketSystem;
    //[SerializeField] private UniqueBuildingsSystem uniqueBuildingsSystem;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playPanel.activeSelf && !isPaused)
            {
                if (marketPanel.activeSelf)
                {
                    //CloseMarketPanel();
                    return;
                }
                if (uniqueBuildingsPanel.activeSelf)
                {
                    //CloseUniqueBuildingsPanel();
                    return;
                }

                PauseGame();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1f;
            Debug.Log("Time scale set to 1x");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2f;
            Debug.Log("Time scale set to 2x");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 6f;
            Debug.Log("Time scale set to 6x");
        }
    }

    private void Start()
    {
        ShowMainMenu();
        UpdateHighScoreText();

        if (saveButton != null)
            saveButton.onClick.AddListener(SaveHighScore);
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        playPanel.SetActive(false);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void ShowPlayPanel()
    {
        gameManager.InitializeGame();
        Debug.Log("New game started");
        menuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        playPanel.SetActive(true);

        marketPanel.SetActive(false);
        uniqueBuildingsPanel.SetActive(false);

        UpdateHighScoreText();
    }

    public void ShowOptionsPanel()
    {
        menuPanel.SetActive(false);
        playPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OpenMarketPanel()
    {
        marketPanel.SetActive(true);
        marketSystem.SetMarketOpen(true);
    }

    public void CloseMarketPanel()
    {
        marketPanel.SetActive(false);
        marketSystem.SetMarketOpen(false);
    }

    public void OpenUniqueBuildingsPanel()
    {
        uniqueBuildingsPanel.SetActive(true);
    }

    public void CloseUniqueBuildingsPanel()
    {
        uniqueBuildingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    public void SaveHighScore()
    {
        int currentDays = gameManager.Days;
        if (currentDays > bestDaysSurvived)
        {
            bestDaysSurvived = currentDays;
            string playerName = nameInputField.text;
            SaveSystem.SaveInt(HIGH_SCORE_KEY, bestDaysSurvived);
            SaveSystem.SaveString(HIGH_SCORE_NAME_KEY, playerName);
            gameManager.ShowNotification("New high score saved!");
        }
        else
        {
            gameManager.ShowNotification("No new high score.");
        }
        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        bestDaysSurvived = SaveSystem.GetInt(HIGH_SCORE_KEY, 0);
        string bestName = SaveSystem.GetString(HIGH_SCORE_NAME_KEY, "");
        if (bestName != "" && bestDaysSurvived > 0)
        {
            highScoreText.text = $"Best: {bestDaysSurvived} days by {bestName}";
        }
        else
        {
            highScoreText.text = "Best: 0 days";
        }
    }
}
