using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Large_Behavior : BasicEnemyBehavior
{
    public bool isStunned;
    public bool hitThePlayerInLastXSec;
    public float hitThePlayerTimer;
    public bool jumpAttackFinished;
    private float stunnDuration;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        renderers  = GetComponentsInChildren<Renderer>();
        lastPosition = transform.position;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        jumpAttackFinished = false;
        hitThePlayerInLastXSec = false;
        stunnDuration = 2f;
        isStunned = false;
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

        if(jumpAttackFinished && !hitThePlayerInLastXSec)
        {
            Debug.Log("BTree wurde deaktiviert!");
            isStunned = true;
            Invoke("EnableBehaviorTree", stunnDuration); 
            gameObject.GetComponent<BehaviorExecutor>().enabled = false;
            jumpAttackFinished = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitThePlayerInLastXSec = true;
            rb.isKinematic = true;
        }
    }

    private void EnableBehaviorTree()
    {
        gameObject.GetComponent<BehaviorExecutor>().enabled = true;
        isStunned = false;
        Debug.Log("BehaviorTree wurde wieder aktiviert!");
    }

    public override void HitByBullet()
    {
        currentHealth -= isStunned ? 20:1;
        
        FlashRed();
    }
}
