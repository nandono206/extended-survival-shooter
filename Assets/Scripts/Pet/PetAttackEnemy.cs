using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class PetAttackEnemy : MonoBehaviour
{
    public string tagtoAttack = "Enemy"; // tag of the objects to attack
    public float speed = 5f; // speed at which the object moves
    public float avoidDistance = 1.5f;
    public float followSpeed = 5f;
    public float radius = 1f;
    [SerializeField]
    private DragonAttackRadius AttackRadius;
    [SerializeField]
    private ParticleSystem ShootingSystem;

    [Space]
    [SerializeField]
    private int BurningDPS = 1000;
    [SerializeField]
    private float BurnDuration = 3f;
    [SerializeField]
    private ParticleSystem OnFireSystemPrefab;

    private ObjectPool<ParticleSystem> OnFirePool;
    public Dictionary<Enemy, ParticleSystem> EnemyParticleSystems = new();
    private Transform attackRadiusTransform;
    private Collider attackRadiusCollider;
    private float detectionRange;
    private GameObject currentTarget;

    Transform target;
    NavMeshAgent nav;
    Vector3 previousPos;
    Animator anim;
    float movementThreshold = 0.0f;


    void Awake()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        OnFirePool = new ObjectPool<ParticleSystem>(CreateOnFireSystem);
        AttackRadius.OnEnemyEnter += StartDamagingEnemy;
        AttackRadius.OnEnemyExit += StopDamagingEnemy;
        
        attackRadiusTransform = transform.Find("AttackRadius");

        // Get the Collider component attached to the AttackRadius transform
        attackRadiusCollider = attackRadiusTransform.GetComponent<Collider>();
        attackRadiusTransform.gameObject.SetActive(false);
        detectionRange = CalculateVolume(attackRadiusCollider);
    }

    private ParticleSystem CreateOnFireSystem()
    {
        return Instantiate(OnFireSystemPrefab);
    }

    private void StartDamagingEnemy(Enemy Enemy)
    {
        if (Enemy.TryGetComponent<EnemyHealth>(out EnemyHealth burnable))
        {
            burnable.StartBurning(BurningDPS);
            Enemy.Health.OnDeath += HandleEnemyDeath;
            ParticleSystem onFireSystem = OnFirePool.Get();
            onFireSystem.transform.SetParent(Enemy.transform, false);
            onFireSystem.transform.localPosition = Vector3.zero;
            ParticleSystem.MainModule main = onFireSystem.main;
            main.loop = true;
            if (EnemyParticleSystems.ContainsKey(Enemy))
            {
                EnemyParticleSystems.Remove(Enemy);
            }
            EnemyParticleSystems.Add(Enemy, onFireSystem);
        }
    }

    private void HandleEnemyDeath(Enemy Enemy)
    {
        Debug.Log("handle enemy death");
        Enemy.Health.OnDeath -= HandleEnemyDeath;
        if (EnemyParticleSystems.ContainsKey(Enemy))
        {
            //StartCoroutine(DelayedDisableBurn(Enemy, EnemyParticleSystems[Enemy], BurnDuration));
            EnemyParticleSystems.Remove(Enemy);
            //Enemy.GetComponent<EnemyHealth>().Death();
        }
    }

    private IEnumerator DelayedDisableBurn(Enemy Enemy, ParticleSystem Instance, float Duration)
    {
        //Debug.Log("delayed disabled burn");
        ParticleSystem.MainModule main = Instance.main;
        main.loop = false;
        yield return new WaitForSeconds(Duration);
        Instance.gameObject.SetActive(false);
        if (Enemy.TryGetComponent<EnemyHealth>(out EnemyHealth burnable))
        {
            burnable.StopBurning();
            
        }
    }



    private void StopDamagingEnemy(Enemy Enemy)
    {
        //Debug.Log("stop burning");
        Enemy.Health.OnDeath -= HandleEnemyDeath;
        if (!Enemy.GetComponent<EnemyHealth>().isDead)
        {
            if (EnemyParticleSystems.ContainsKey(Enemy))
            {
                StartCoroutine(DelayedDisableBurn(Enemy, EnemyParticleSystems[Enemy], BurnDuration));
                EnemyParticleSystems.Remove(Enemy);
            }
        }
       
        
    }
    


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<PetController>().Health > 0 && target.GetComponent<PlayerHealth>().currentHealth > 0)
        {
            Vector3 playerPos = target.position;

            Vector3 center = playerPos;
            Vector3 direction = transform.position - center;
                
            Vector3 followDirection = new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z);



           


            // Search for valid target game object
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
            Vector3 avoidDirection = Vector3.zero;


            if (currentTarget ==  null)
            {
                float minDistance = float.PositiveInfinity;
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag(tagtoAttack))
                    {
                        // Set new target and reset attack cooldown
                        if (Vector3.Distance(transform.position, collider.transform.position) < minDistance)
                        {
                            minDistance = Vector3.Distance(transform.position, collider.transform.position);
                            currentTarget = collider.gameObject;
                        }
                        
                    }
                }
            }
            

           
            

            // If current target exists, lock onto target and attack
            if (currentTarget != null)
            {
                // Lock onto target
                Vector3 targetDirection = currentTarget.transform.position;
                Vector3 movement = ((targetDirection - transform.position)).normalized * followSpeed;
                movement.y = 0f;
                if (Vector3.Distance(targetDirection, transform.position) > 7f)
                {
                    nav.SetDestination(transform.position + movement);
                }

                targetDirection.y = 0f; // ignore the y-axis component
                //transform.rotation = Quaternion.LookRotation(targetDirection);
                if (!currentTarget.GetComponent<EnemyHealth>().isSinking)
                {
                    transform.LookAt(currentTarget.transform);
                }
               
                // Attack the target
                ShootingSystem.gameObject.SetActive(true);
                AttackRadius.gameObject.SetActive(true);
            }
            else
            {
                AttackRadius.gameObject.SetActive(false);
                ShootingSystem.gameObject.SetActive(false);
                // Move towards a default direction or avoid direction if needed
                Vector3 movement = (followDirection).normalized * followSpeed;
                movement.y = 0f; // ignore the y-axis component
                nav.SetDestination(transform.position + movement);
            }

        }
        else
        {
            nav.enabled = false;
        }

        if (anim != null)
        {
            if (nav.velocity.magnitude > movementThreshold)
            {

                anim.SetBool("IsWalking", true);
            }
            else
            {
                //Debug.Log("char is Idle");
                anim.SetBool("IsWalking", false);
            }
        }

        previousPos = transform.position;
    }
    float CalculateVolume(Collider collider)
    {
        Vector3 size = collider.bounds.size;
        return size.x * size.y * size.z;
    }
}
