using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    public string enemyName;
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;  // Sink through the floor (so it doesn't magically dissapear)
    public int scoreValue = 10;
    public AudioClip deathClip;
    private Coroutine BurnCoroutine;

    [SerializeField]
    private bool _IsBurning;
    public bool IsBurning { get => _IsBurning; set => _IsBurning = value; }

    public event DeathEvent OnDeath;
    public delegate void DeathEvent(Enemy Enemy);


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    public bool isDead;
    public bool isSinking;


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

    public void StartBurning(int DamagePerSecond)
    {
        IsBurning = true;
        if (BurnCoroutine != null)
        {
            StopCoroutine(BurnCoroutine);
        }

        BurnCoroutine = StartCoroutine(Burn(DamagePerSecond));
    }

    private IEnumerator Burn(int DamagePerSecond)
    {
        float minTimeToDamage = 1f / DamagePerSecond;
        WaitForSeconds wait = new WaitForSeconds(minTimeToDamage);
        int damagePerTick = Mathf.FloorToInt(minTimeToDamage) + 1;

        TakeDamageBurn(damagePerTick);
        while (IsBurning)
        {
            yield return wait;
            TakeDamageBurn(damagePerTick);
        }
    }

    public void TakeDamageBurn(int Damage)
    {

        if (isDead)
        {
            return;
        }
        // Tidak perlu melakukan apapun


        //enemyAudio.Play();
        //Debug.Log("BURNING");
        Debug.Log(currentHealth);
        currentHealth -= Damage;

        
        if (currentHealth <= 0)
        {
            isDead = true;
            //StopBurning();
            Debug.Log(currentHealth);
            Debug.Log("still burning take burn");
            OnDeath?.Invoke(GetComponent<Enemy>());
            StopBurning();
            //yield return new WaitForSeconds(3f);
            Death();
            
        }
    }

    public void StopBurning()
    {
        Debug.Log("Stop Burning");
        IsBurning = false;
        if (BurnCoroutine != null)
        {
            StopCoroutine(BurnCoroutine);
        }
        
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if (isDead)
        {
            return;
        }
            // Tidak perlu melakukan apapun
            

        enemyAudio.Play ();

        currentHealth -= amount;
        Debug.Log(currentHealth);


        // Cari posisi hit point lalu munculkan partikel
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
        {
            isDead = true;
            if (IsBurning)
            {
                Debug.Log("still burning");
                OnDeath?.Invoke(GetComponent<Enemy>());
                StopBurning();
                Death ();

            }
            else
            {
                Death();
            }
            
        }
    }


    public void Death ()
    {
       
        isDead = true;
        //if (gameObject.GetComponent<>().EnemyParticleSystems.ContainsKey())

        // Membuat musuh tidak menjadi obstacle
        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");
        Debug.Log("DEAD");
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
        Debug.Log("SINK");
        Destroy (gameObject, 2f);
    }
}
