using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    // RotateDoor ima collider i istrigger, Player ima rigidbody i collider
    private bool _shouldRotate = true;

    private void Update()
    {
        RotateTheDoor();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            _shouldRotate = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            _shouldRotate = true;
        }
    }

    private void RotateTheDoor()
    {
        if (_shouldRotate)
        {
            transform.Rotate(Vector3.up, 90f * Time.deltaTime);
        }
    }
}
