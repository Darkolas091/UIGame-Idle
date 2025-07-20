using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerScript2 player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPosition1;
    [SerializeField] private Transform spawnPosition2;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private TMP_Text health;

    private bool _isGameOver = false;
    private Coroutine _spawnEnemyCoroutine;
    private int _score = 0;
    private List<GameObject> _enemies = new List<GameObject>();
    private int _highScore = 0;

    public bool IsGameOver => _isGameOver;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();
    }

    public void StartGame()
    {
        _isGameOver = false;
        _score = 0;
        ClearEnemies();
        _spawnEnemyCoroutine = StartCoroutine(SpawnEnemyRoutine());
        player.transform.position = new Vector3(2.5f, -0.25f, 0f);
        scoreText.gameObject.SetActive(true);
        health.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(false);
        UpdateScoreText();
        UpdateHighScoreText();
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (!_isGameOver)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy1 = Instantiate(enemyPrefab, spawnPosition1.position, Quaternion.Euler(0, 180, 0));
        enemy1.GetComponent<Enemy>().Initialize(this, player, -1);
        _enemies.Add(enemy1);

        GameObject enemy2 = Instantiate(enemyPrefab, spawnPosition2.position, Quaternion.identity);
        enemy2.GetComponent<Enemy>().Initialize(this, player, 1);
        _enemies.Add(enemy2);
    }

    public void GameOver()
    {
        _isGameOver = true;
        scoreText.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);
        if (_spawnEnemyCoroutine != null)
        {
            StopCoroutine(_spawnEnemyCoroutine);
            _spawnEnemyCoroutine = null;
        }
        ClearEnemies();

        if (_score > _highScore)
        {
            _highScore = _score;
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save();
            UpdateHighScoreText();
        }

            menuManager.HandleGameOver();
        
    }

    private void ClearEnemies()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        _enemies.Clear();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        Debug.Log("Score: " + _score);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
            scoreText.text = "Score: " + _score;
    }

    private void UpdateHighScoreText()
    {
            highScoreText.text = "High Score: " + _highScore;
    }
}
