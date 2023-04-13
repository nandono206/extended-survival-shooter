using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public string enemyName;
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;  // Sink through the floor (so it doesn't magically dissapear)
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if (isSinking)
        {
            // Enemy bergerak ke dalam lantai saat mati
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if (isDead)
            // Tidak perlu melakukan apapun
            return;

        enemyAudio.Play ();

        currentHealth -= amount;

        // Cari posisi hit point lalu munculkan partikel
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        // Membuat musuh tidak menjadi obstacle
        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

    // Dipanggil di Zombunny > Animation > Death > Evemts
    public void StartSinking ()
    {
        // .enabled: merujuk ke hanya game object ini dan tidak seluruh game object
        GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent<Rigidbody> ().isKinematic = true;
        isSinking = true;
        QuestManager.AddEnemyKilled(enemyName);
        Destroy (gameObject, 2f);
    }
}
