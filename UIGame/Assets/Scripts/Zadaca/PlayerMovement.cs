    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
    // Player ima rigidboy i collider, lockane constraints na rotaciju, te yz, SlowFog ima collider i istrigger, skripta od SlowFog je prazna
    [SerializeField] private float speed = 5f;
        [SerializeField] private Rigidbody rigidBody;
        private float _currentSpeed = 0f;

        private void Start()
        {
            _currentSpeed = speed;
        }


        private void Update()
        {
            Move();
        }

        private void Move()
        {
            rigidBody.linearVelocity = Vector3.right * _currentSpeed;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<SlowFog>(out SlowFog slowFog))
            {
                _currentSpeed = speed * 0.2f;
            Debug.Log("Player entered slow fog, speed reduced to: " + _currentSpeed);
        }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<SlowFog>(out SlowFog slowFog))
            {
                _currentSpeed = speed;
            Debug.Log("Player exited slow fog, speed restored to: " + _currentSpeed);
        }
        }
    }
