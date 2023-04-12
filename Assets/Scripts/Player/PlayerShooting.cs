using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour, PetObserver
{
    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.15f;        
    public float range = 100f;                      

    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;
    GameObject pet;
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;
    GameObject spawener;
    PetSubject petSubject;
    Light gunLight;                                 
    float effectsDisplayTime = 0.2f;                

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
    
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        spawener = GameObject.FindGameObjectWithTag("Spawner");
        petSubject = spawener.GetComponent<Spawner>();
        if (petSubject != null)
        {

            petSubject.AddObserver(this);

        }
    }

    public void OnNotify(string petTag)
    {

        pet = GameObject.FindGameObjectWithTag(petTag);

    }

    public void OnNotifyDead()
    {
        //Debug.Log("dragon dead");
        pet = null;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (pet != null && pet.CompareTag("Dragon"))
        {
            //Debug.Log("Dragon Exists");
            damagePerShot = 100;
            gunLine.material.color = Color.red;
        }

        else 
        {
            //Debug.Log("Dragon Exists");
            damagePerShot = 20;
            gunLine.material.color = Color.yellow;
        }

        // Fire1: left ctrl/mouse 0
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position); // Start position

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;
        

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}