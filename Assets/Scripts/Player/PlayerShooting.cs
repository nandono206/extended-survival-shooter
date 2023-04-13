using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour, PetObserver
{
    public int damagePerGunShot = 20;
    public int damagePerShotgunShot = 10;
    public int damagePerSwordHit = 50;
    public float maxBowPower = 100f;
    public float bowChargeRate = 10f;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public float spreadAngle = 50f;
    public int numOfShotgunBullets = 8;
    // booleans for if the weapon has been bought
    public static bool isShotgunAvailable = false;
    public static bool isSwordAvailable = false;
    public static bool isBowAvailable = false;
    public bool areOtherWeaponsActive = false;
    public GameObject arrowPrefab;

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
    int activeWeapon = 0;
    bool isBowCharging = false;
    float currentBowPower = 0f;

    LineRenderer[] gunLines;

    GameObject gun;
    GameObject shotgun;
    GameObject sword;
    GameObject bow;
    GameObject arrow;
    BoxCollider swordCollider;
    GameObject bowBar;
    GameObject bowChargeBar;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
    
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        gun = GameObject.Find("Gun");
        shotgun = GameObject.Find("Shotgun");
        sword = GameObject.Find("Sword");
        bow = GameObject.Find("Bow");
        shotgun.SetActive(false);
        sword.SetActive(false);
        bow.SetActive(false);
        swordCollider = GameObject.Find("SwordCollider").GetComponent<BoxCollider>();
        swordCollider.enabled = false;
        bowChargeBar = GameObject.Find("BowChargeBar");
        bowBar = GameObject.Find("BowBar");
        bowChargeBar.transform.localScale = new Vector3(0, 1, 1);
        bowBar.SetActive(false);

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
            damagePerGunShot = 40;
            damagePerShotgunShot = 20;
            damagePerSwordHit = 90;

            gunLine.material.color = Color.red;
        }

        else 
        {
            //Debug.Log("Dragon Exists");
            damagePerGunShot = 20;
            damagePerShotgunShot = 10;
            damagePerSwordHit = 50;
            gunLine.material.color = Color.yellow;
        }

        // Fire1: left ctrl/mouse 0
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }

        if (isBowCharging)
        {
            currentBowPower += bowChargeRate * Time.deltaTime;
            currentBowPower = Mathf.Min(currentBowPower, maxBowPower);
            bowChargeBar.transform.localScale = new Vector3(currentBowPower / maxBowPower, 1, 1);
        }

        if (Input.GetButtonUp("Fire1") && isBowCharging)
        {
            ShootBow();
            bowChargeBar.transform.localScale = new Vector3(0, 1, 1);
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

        bool[] weaponAvailable = new bool[] { true, isShotgunAvailable, isSwordAvailable, isBowAvailable };
        if (Input.GetKeyDown(KeyCode.Alpha1))
            activeWeapon = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2) && isShotgunAvailable)
            activeWeapon = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3) && isSwordAvailable)
            activeWeapon = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4) && isBowAvailable)
            activeWeapon = 3;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                activeWeapon--;
                if (activeWeapon < 0)
                    activeWeapon = 3;
            }
            else
            {
                activeWeapon++;
                if (activeWeapon > 3)
                    activeWeapon = 0;
            }
        }

        if (activeWeapon >= 0 && activeWeapon <= 3 && weaponAvailable[activeWeapon])
        {
            gun.SetActive(activeWeapon == 0);
            shotgun.SetActive(activeWeapon == 1);
            sword.SetActive(activeWeapon == 2);
            bow.SetActive(activeWeapon == 3);

            areOtherWeaponsActive = activeWeapon == 1; // make true for 2 and 3 when animations are ready

            bowBar.SetActive(activeWeapon == 3);
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
        if (gunLines != null)
        {
            for (int i = 0; i < gunLines.Length; i++)
            {
                gunLines[i].enabled = false;
            }
        }
    }

    void Shoot()
    {
        timer = 0f;
        if (activeWeapon == 0 || activeWeapon == 1)
        {
            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            if (activeWeapon == 0)
            {
                ShootGun();
            }
            else if (activeWeapon == 1)
            {
                ShootShotgun();
            }
        }
        else if (activeWeapon == 2)
        {
            SwingSword();
        }
        else if (activeWeapon == 3)
        {
            isBowCharging = true;
        }
    }

    void ShootGun()
    {
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position); // Start position

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;
        

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerGunShot, shootHit.point);
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    void ShootShotgun()
    {
        gunLines = new LineRenderer[numOfShotgunBullets];
        for (int i = 0; i < numOfShotgunBullets; i++)
        {
            gunLines[i] = GameObject.Find("Shotgun").transform.GetChild(i).GetComponent<LineRenderer>();
            gunLines[i].enabled = true;
            gunLines[i].SetPosition(0, transform.position);

            Vector3 direction = Quaternion.Euler(0, Random.Range(-spreadAngle, spreadAngle), 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, range, shootableMask))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.point);
                    float damageMultiplier = Mathf.Clamp01(1 - distance / (range * 0.2f));
                    enemyHealth.TakeDamage((int)(damagePerShotgunShot * damageMultiplier), hit.point);
                }

                gunLines[i].SetPosition(1, hit.point);
            }
            else
            {
                gunLines[i].SetPosition(1, transform.position + direction * range * 0.3f);
            }
        }
    }

    void SwingSword()
    {
        swordCollider.enabled = true;
        sword.GetComponent<Animator>().Play("SwordAnimation");
        // Check for collisions with enemies
        Collider[] hitColliders = Physics.OverlapBox(swordCollider.bounds.center, swordCollider.bounds.extents, swordCollider.transform.rotation, shootableMask);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerSwordHit, hitCollider.transform.position);
            }
        }
        swordCollider.enabled = false;
    }

    void ShootBow()
    {
        isBowCharging = false;
        arrow = Instantiate(arrowPrefab, transform.position, transform.rotation * Quaternion.Euler(0, 90, -100)) as GameObject;
        arrow.GetComponent<Rigidbody>().AddForce(transform.forward * currentBowPower, ForceMode.Impulse);
        currentBowPower = 0f;
    }
}