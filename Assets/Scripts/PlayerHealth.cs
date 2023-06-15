using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        Debug.Log("Health: " + maxHealth);
        currentHealth = maxHealth;    
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 5;
            Debug.Log("Health: " + currentHealth);
        }

        if(collider.gameObject.CompareTag("Rocket"))
        {
            currentHealth -= 10;
            Debug.Log("Health: " + currentHealth);
            Destroy(collider.gameObject);
        }

        if(collider.gameObject.CompareTag("Enemy_Large"))
        {
            currentHealth -= 20;
            Debug.Log("Health: " + currentHealth);
        }
    }
}
