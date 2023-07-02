using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;
    public bool isHitByKick;
    public bool isHitByLaser;
    public bool isHitByJump;
    public float damageShockWave;

    private GameObject Canvas;
    private GameObject healthbarCanvas;
    private GameObject background;
    private GameObject foreground;
    private Image foregroundSprite;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("P_LPSP_UI_Canvas(Clone)");
        healthbarCanvas = Canvas.transform.Find("HealthBarCanvas").gameObject;
        background = healthbarCanvas.transform.Find("Background").gameObject;
        foreground = background.transform.Find("Foreground").gameObject;
        foregroundSprite = foreground.GetComponent<Image>();
        maxHealth = 100;
        currentHealth = maxHealth;  
        foregroundSprite.fillAmount = currentHealth/maxHealth;  

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
            foregroundSprite.fillAmount = currentHealth/maxHealth;
            damageShockWave = 0f;
            isHitByJump = false;
        }
        if(isHitByLaser)
        {
            currentHealth -= 0.5f; 
            foregroundSprite.fillAmount = currentHealth/maxHealth;
            isHitByLaser = false;
        }
        if(isHitByKick)
        {
            Debug.Log("JETZT!!!!!!");
            currentHealth -= 20; 
            foregroundSprite.fillAmount = currentHealth/maxHealth;
            isHitByKick = false;
        }
        
        if(currentHealth <= 0)
        {
            SceneManager.LoadScene("Death Scene");
            //Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 5;
            foregroundSprite.fillAmount = currentHealth/maxHealth;
        }

        if(collider.gameObject.CompareTag("Rocket"))
        {
            currentHealth -= 10;
            foregroundSprite.fillAmount = currentHealth/maxHealth;
            Destroy(collider.gameObject);
        }

        if(collider.gameObject.CompareTag("Enemy_Large"))
        {
            currentHealth -= 10;
            foregroundSprite.fillAmount = currentHealth/maxHealth;
        }
    }
}
