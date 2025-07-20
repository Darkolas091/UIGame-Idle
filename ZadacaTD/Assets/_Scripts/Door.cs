using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen { get; private set; } = false;
    public void Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            gameObject.SetActive(false);
            Debug.Log("Door opened!");
        }
    }
}
