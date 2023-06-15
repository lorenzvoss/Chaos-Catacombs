using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : MonoBehaviour
{
    public int maxHealth;
    public bool isHit;

    protected Renderer[] renderers;
    protected int currentHealth;
    protected Dictionary<Renderer, Color[]> originalEmissionColors = new Dictionary<Renderer, Color[]>();
    protected float emissionIntensity = 1.5f; // Adjust the emission intensity as needed
    protected Vector3 lastPosition;
    protected Animator animator;
    protected Rigidbody rb;
    protected GameObject player;



    private void Start()
    {
        maxHealth = 90;
        currentHealth = maxHealth;
        renderers  = GetComponentsInChildren<Renderer>();
        lastPosition = transform.position;
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        isHit = false;
    }
    private void Update()
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

    public virtual void HitByBullet()
    {
        // Decrease current health
        currentHealth -= 30;
        
        FlashRed();
    }


    public void FlashRed()
    {
        // Save the original emission colors if they haven't been stored yet
        if (originalEmissionColors.Count == 0)
        {
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                Color[] emissionColors = new Color[materials.Length];

                for (int i = 0; i < materials.Length; i++)
                {
                    emissionColors[i] = materials[i].GetColor("_EmissionColor");
                }

                originalEmissionColors[renderer] = emissionColors;
            }
        }

        // Adjust the emission properties
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                mat.SetColor("_EmissionColor", Color.red * emissionIntensity);
                mat.EnableKeyword("_EMISSION");
            }
        }

        // Call the method to reset the emission after a certain delay
        StartCoroutine(ResetEmissionAfterDelay(0.11f)); // Adjust the delay as needed
    }

    public IEnumerator ResetEmissionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Restore the original emission colors
        foreach (Renderer renderer in renderers)
        {
            if (originalEmissionColors.ContainsKey(renderer))
            {
                Color[] emissionColors = originalEmissionColors[renderer];

                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    renderer.materials[i].SetColor("_EmissionColor", emissionColors[i]);
                }
            }
        }

        // Clear the dictionary of original emission colors
        originalEmissionColors.Clear();
    }

}
