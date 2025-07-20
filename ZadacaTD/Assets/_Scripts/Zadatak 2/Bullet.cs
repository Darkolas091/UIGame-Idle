using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(DestroyAfter5());
    }

    IEnumerator DestroyAfter5()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
