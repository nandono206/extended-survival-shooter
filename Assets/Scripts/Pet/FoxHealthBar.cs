using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxHealthBar : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider foxHealthSlider;

    bool isDead;
    //bool damaged;




    void Awake()
    {
        

        currentHealth = startingHealth;
    }

    // If damage taken, set damageImage to flash colour
    void Update()
    {
        
        //damaged = false;
    }

    // Method yang dipanggil jika musuh menyerang player
    public void TakeDamage(int amount)
    {
        //damaged = true;

        currentHealth -= amount;

        foxHealthSlider.value = currentHealth;

        //TODO: ADD AUDIO UPON TAKING DAMAGE

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    // Jika player mati
    void Death()
    {
        //isDead = true;

        //playerShooting.DisableEffects();

        //anim.SetTrigger("Die");

        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        //playerMovement.enabled = false;
        //playerShooting.enabled = false;
        isDead = true;
        Debug.Log("Dead");
    }
}
