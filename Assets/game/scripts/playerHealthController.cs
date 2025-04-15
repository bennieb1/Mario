using System;
using UnityEngine;

public class playerHealthController : Singelton<playerHealthController>
{

    

    public float currentHealth, maxHealth;

    public float invincableLength;

    public float invincableCounter;

    public SpriteRenderer sr;
    
    public Animator anim;

    //public GameObject deathEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

    }

  

    // Update is called once per frame
    void Update()
    {
        if (invincableCounter > 0)
        {
            invincableCounter -= Time.deltaTime;

            if (invincableCounter <=0)
            {
                
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
                
            }
        }
    }

    public void DealDamage()
    {

        if (invincableCounter <= 0)
        {

            currentHealth--;
           
            if (currentHealth <= 0)
            {
                currentHealth = 0;
               // gameObject.SetActive(false);
               
               //Instantiate(deathEffect, transform.position, transform.rotation);
                
            }
            else
            {
                invincableCounter = invincableLength;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);
                PlayerController.Instance.KnockBack();
              
                //AudioManager.Instance.PlaySfx(9);
            }

            
        }
    }

    public void HealPlayer()
    {
        MarioSizeController.Instance.Grow();
        anim.SetBool("Mushroom", true);
    }
}
