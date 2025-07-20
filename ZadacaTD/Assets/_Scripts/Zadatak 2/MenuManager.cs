using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        mainMenuPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        mainMenuPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        gameManager.StartGame();
    }

    public void RestartGame()
    {
        gameOverPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
    }

    public void HandleGameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
}
