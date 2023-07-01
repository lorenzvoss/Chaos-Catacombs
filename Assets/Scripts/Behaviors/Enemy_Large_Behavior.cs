using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        healthbarCanvas = gameObject.transform.Find("HealthBarCanvas").gameObject;
        background = healthbarCanvas.transform.Find("Background").gameObject;
        foreground = background.transform.Find("Foreground").gameObject;
        foregroundSprite = foreground.GetComponent<Image>();
        currentHealth = maxHealth;
        foregroundSprite.fillAmount = currentHealth/maxHealth;
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
        healthbarCanvas.transform.LookAt(player.transform);

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
            Invoke("ResetHitPlayer", 2);
            rb.isKinematic = true;
        }
        rb.isKinematic = true;
    }

    private void EnableBehaviorTree()
    {
        gameObject.GetComponent<BehaviorExecutor>().enabled = true;
        isStunned = false;
    }

    public override void HitByBullet()
    {
        currentHealth -= isStunned ? 20:1;
        foregroundSprite.fillAmount = currentHealth/maxHealth;
        FlashRed();
    }

    public void ResetHitPlayer()
    {
        hitThePlayerInLastXSec = false;
    }
}
