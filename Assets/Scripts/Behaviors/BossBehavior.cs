using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossBehavior : BasicEnemyBehavior
{
    public bool wasShotInHead;
    

    // Start is called before the first frame update
    void Start()
    {
        healthbarCanvas = gameObject.transform.Find("HealthBarCanvas").gameObject;
        background = healthbarCanvas.transform.Find("Background").gameObject;
        foreground = background.transform.Find("Foreground").gameObject;
        foregroundSprite = foreground.GetComponent<Image>();
        maxHealth = 1000;
        currentHealth = maxHealth;
        foregroundSprite.fillAmount = currentHealth/maxHealth;
        renderers  = GetComponentsInChildren<Renderer>();
        lastPosition = transform.position;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        wasShotInHead = false;
        

        Debug.Log("Boss: " + currentHealth);
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
            //Destroy(gameObject);
            SceneManager.LoadScene("FinishScene");
        }
    }

    public override void HitByBullet()
    {
        currentHealth -= wasShotInHead ? 30:10;
        Debug.Log(currentHealth/maxHealth);
        foregroundSprite.fillAmount = currentHealth/maxHealth;
        wasShotInHead = false;
        Debug.Log("Boss: " + currentHealth);        
        FlashRed();
    }


}
