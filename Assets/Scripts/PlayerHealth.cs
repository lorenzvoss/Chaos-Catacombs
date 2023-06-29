using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    public bool isHitByKick;
    public bool isHitByLaser;
    public bool isHitByJump;
    public float damageShockWave;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        Debug.Log("Health: " + maxHealth);
        currentHealth = maxHealth;    

        isHitByJump = false;
        isHitByLaser = false;
        isHitByKick = false;
        damageShockWave = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHitByJump)
        {
            currentHealth -= damageShockWave; 
            Debug.Log("Health: " + currentHealth);
            damageShockWave = 0f;
            isHitByJump = false;
        }
        if(isHitByLaser)
        {
            currentHealth -= 3; 
            Debug.Log("Health: " + currentHealth);
        }
        if(isHitByKick)
        {
            currentHealth -= 20; 
            Debug.Log("Health: " + currentHealth);
            isHitByKick = false;
        }
        
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
