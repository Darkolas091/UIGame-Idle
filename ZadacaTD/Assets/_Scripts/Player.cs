using UnityEngine;

public class Player : MonoBehaviour
{
    public float _moveSpeed = 5f;
    private bool _hasKey = false;
    private Door _currentDoor = null;

    void Update()
    {
        float _moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 _movement = new Vector3(_moveInput, 0f, 0f) * _moveSpeed * Time.deltaTime;
        transform.position += _movement;

        if (_currentDoor != null && Input.GetKeyDown(KeyCode.E))
        {
            TryOpenDoor();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Key keyComponent;
        if (other.TryGetComponent<Key>(out keyComponent))
        {
            if (!_hasKey)
            {
                _hasKey = true;
                Destroy(other.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Door doorComponent;
        if (collision.collider.TryGetComponent<Door>(out doorComponent))
        {
            _currentDoor = doorComponent;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Door doorComponent;
        if (collision.collider.TryGetComponent<Door>(out doorComponent))
        {
            if (_currentDoor == doorComponent)
            {
                _currentDoor = null;
            }
        }
    }

    private void TryOpenDoor()
    {
        if (_hasKey)
        {
            _currentDoor.Open();
            _hasKey = false;
            Debug.Log("Door opened!");
        }
        else
        {
            Debug.Log("Need a key to open the door!");
        }
    }
}
