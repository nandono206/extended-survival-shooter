using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour, PetObserver
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    
    GameObject pet;
    GameObject spawener;
    FoxHealthBar petHealth;
    PlayerHealth playerHealth;
  

    EnemyHealth enemyHealth;
   
    bool playerInRange;
    bool petInRange;
    float timer;

    [SerializeField] PetSubject petSubject;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
        spawener = GameObject.FindGameObjectWithTag("Spawner");
        petSubject = spawener.GetComponent <Spawner> ();
        if ( petSubject != null )
        {
            
            petSubject.AddObserver(this);
          
        }
    }


    // Jika sesuatu collide dengan enemy
    void OnTriggerEnter (Collider other)
    {
        // Jika player, playerinrange true
        if(other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = true;
        }
        
        
        if (other.gameObject == pet && other.isTrigger == false)
        {
            petInRange = true;
        }
    }

    // Kebalikan OnTriggerEnter
    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }

        

        if (other.gameObject == pet)
        {
            petInRange = false;
        }

    }


    void Update ()
    {
        if (pet == null)
        {
            pet = GameObject.FindGameObjectWithTag("Fox");
            if (pet != null)
            {
                return;
            }

            pet = GameObject.FindGameObjectWithTag("Dragon");
            if (pet != null)
            {
                return;
            }

            pet = GameObject.FindGameObjectWithTag("Bear");
            if (pet != null)
            {
                return;
            }
        }

        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
        
            Attack ();
        }

        if (timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0 && petInRange)
        {
          
            AttackPet ();
        }
       
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }

    public void OnNotify(string petTag)
    {
     
        pet = GameObject.FindGameObjectWithTag(petTag);
       
    }

    public void OnNotifyDead()
    {
        pet = null;
        petInRange = false;
    }


    void Attack ()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
           
            playerHealth.TakeDamage (attackDamage);
        }
    }
    
    void AttackPet()
    {
        timer = 0f;

        if(pet != null)
        {
            if (pet.GetComponent<PetController>().Health > 0)
            {
                //Debug.Log("AttackingPet " + pet.tag);
                pet.GetComponent<PetController>().OnTakeDamage (attackDamage);
            }
        }
    }

    //void AttackFox()
    //{
    //    timer = 0f;

    //    if (foxHealth.currentHealth > 0)
    //    {
    //        Debug.Log("AttackFox");
    //        foxHealth.TakeDamage(attackDamage);
    //    }
    //}
    //void AttackDragon()
    //{
    //    timer = 0f;

    //    if (dragonHealth.currentHealth > 0)
    //    {
            
    //        dragonHealth.TakeDamage(attackDamage);
    //    }
    //}




}
