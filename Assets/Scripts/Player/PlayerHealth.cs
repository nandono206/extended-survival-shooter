using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip healClip;
    public AudioClip damageClip;
    public AudioClip deathClip;
    public float flashSpeed = 5f;       // Kecepatan DamageImage flash ke layar
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // Warna flash
    public float healFlashSpeed = 5f;
    public Color healFlashColor = new Color(0f, 1f, 0f, 0.1f);
    public Image healImage;


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    bool isHealingfromPet;
    


    void Awake()
    {
        anim = GameObject.Find("Alice").GetComponent<Animator>(); ;
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();

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

        if (isHealingfromPet)
        {
            healImage.color = healFlashColor;
        }
        else
        {
            healImage.color = Color.Lerp(healImage.color, Color.clear, healFlashSpeed * Time.deltaTime);
        }

        isHealingfromPet = false;
        damaged = false;
    }

    // Method yang dipanggil jika musuh menyerang player
    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.clip = damageClip;
        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void HealFromPet()
    {
        isHealingfromPet = true;

        currentHealth += 5;

        healthSlider.value = currentHealth;

        playerAudio.clip = healClip;
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

        playerShooting.DisableEffects();

        GameObject gun = GameObject.Find("Gun");
        if (gun != null)
            gun.SetActive(false);
        GameObject shotgun = GameObject.Find("Shotgun");
        if (shotgun != null)
            shotgun.SetActive(false);
        GameObject sword = GameObject.Find("Sword");
        if (sword != null)
            sword.SetActive(false);
        GameObject bow = GameObject.Find("Bow");
        if (bow != null)
            bow.SetActive(false);
        anim.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }
}
