using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;       // Kecepatan DamageImage flash ke layar
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // Warna flash


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    bool isDead;
    bool damaged;    


    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        //playerShooting = GetComponentInChildren<PlayerShooting>();

        currentHealth = startingHealth;
    }

    // If damage taken, set damageImage to flash colour
    void Update()
    {
        if (damaged)
        {
            // Ubah menjadi merah
            damageImage.color = flashColour;
        }
        else
        {
            // Fade damage image menjadi transparan
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    // Method yang dipanggil jika musuh menyerang player
    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    // Jika player mati
    void Death()
    {
        isDead = true;

        //playerShooting.DisableEffects();

        anim.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }
}
