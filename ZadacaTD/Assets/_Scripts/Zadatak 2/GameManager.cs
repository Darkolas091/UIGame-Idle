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

    private bool _isGameOver = false;
    private Coroutine _spawnEnemyCoroutine;
    private int _score = 0;

    private List<GameObject> _enemies = new List<GameObject>();

    void Start()
    {
        _spawnEnemyCoroutine = StartCoroutine(SpawnEnemyRoutine());
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
        // moving left
        GameObject enemy1 = Instantiate(enemyPrefab, spawnPosition1.position, Quaternion.Euler(0, 180, 0));
        enemy1.GetComponent<Enemy>().Initialize(this, player, -1);
        _enemies.Add(enemy1);

        // moving right
        GameObject enemy2 = Instantiate(enemyPrefab, spawnPosition2.position, Quaternion.identity);
        enemy2.GetComponent<Enemy>().Initialize(this, player, 1);
        _enemies.Add(enemy2);
    }

    public void GameOver()
    {
        _isGameOver = true;
        if (_spawnEnemyCoroutine != null)
        {
            StopCoroutine(_spawnEnemyCoroutine);
            _spawnEnemyCoroutine = null;
        }

        // Destroy all enemies and clear the list
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
    }
}
