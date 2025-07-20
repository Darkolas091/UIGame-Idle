using UnityEngine;

public class PlayerScript2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField] private int maxHP = 5;
    [SerializeField] private int currentHP;

    [Header("References")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameManager gameManager;

    private Rigidbody2D _rb;
    private Vector2 _movement;
    private bool _facingRight = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");

        if (_movement.x > 0 && !_facingRight)
            Flip();
        else if (_movement.x < 0 && _facingRight)
            Flip();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_movement.x * moveSpeed, _rb.linearVelocity.y);
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Shoot()
    {
        GameObject _bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        Rigidbody2D bulletRb = _bullet.GetComponent<Rigidbody2D>();

        Vector2 direction;
        if (_facingRight)
            direction = Vector2.right;
        else
            direction = Vector2.left;
        bulletRb.linearVelocity = direction * 15f;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP < 0)
        {
            gameManager.GameOver();
        }
            
    }
}
