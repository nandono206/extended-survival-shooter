using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxHeal : MonoBehaviour
{

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;

    bool playerInRange;
    float timer;


    void Awake()
    {
        //Mencari game object dengan tag "Player"
        player = GameObject.FindGameObjectWithTag("Player");

        //mendapatkan komponen player health
        playerHealth = player.GetComponent<PlayerHealth>();

        //mendapatkan komponen Animator
        anim = GetComponent<Animator>();
        //enemyHealth = GetComponent<EnemyHealth>();  
        playerInRange = true;    
    }


    


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 10.0f && playerInRange && playerHealth.currentHealth < 100 /* && enemyHealth.currentHealth > 0*/)
        {
            Heal();
        }

        //mentrigger animasi PlayerDead jika darah player kurang dari sama dengan 0
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("PlayerDead");
        }
    }


    void Heal()
    {
        //Reset timer
        timer = 0f;

        //Taking Damage
        if (playerHealth.currentHealth > 0 && playerHealth.currentHealth < 100)
        {
            playerHealth.HealFromPet();
        }
    }
}
