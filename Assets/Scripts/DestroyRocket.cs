using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMe", 3f);
    }

    // Update is called once per frame
    void DestroyMe()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collider)
    {
        if(!collider.gameObject.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
