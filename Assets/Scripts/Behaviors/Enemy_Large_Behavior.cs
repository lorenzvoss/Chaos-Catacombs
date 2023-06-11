using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Large_Behavior : BasicEnemyBehavior
{
    public bool isStunned;
    public bool hitThePlayerInLastXSec;
    public float hitThePlayerTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        renderers  = GetComponentsInChildren<Renderer>();
        lastPosition = transform.position;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");

        hitThePlayerTimer = 1.5f;
        hitThePlayerInLastXSec = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit == true)
        {
            HitByBullet();
            isHit = false;
        }

        // Check if enemy is still alive
        if (currentHealth <= 0)
        {
            animator.SetBool("isAlive", false);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitThePlayerInLastXSec = true;
            hitThePlayerTimer = 1.5f;
            Debug.Log("Hit the Player is true");
        }
    }
}
