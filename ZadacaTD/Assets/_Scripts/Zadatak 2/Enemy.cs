using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _moveSpeed = 5f;
    private int _direction = 1;
    private PlayerScript2 _player;
    private GameManager _gameManager;

    public void Initialize(GameManager gameManager, PlayerScript2 player, int direction)
    {
        _gameManager = gameManager;
        _player = player;
        _direction = direction;
    }

    void Update()
    {
        transform.position += Vector3.right * _direction * _moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerScript2>(out var player))
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Bullet>(out var bullet))
        {
            _gameManager.AddScore(1);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
