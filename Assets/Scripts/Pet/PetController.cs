using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PetController : PetSubject, IDamageable
{
    // Start is called before the first frame update
    [SerializeField]
    public int Health = 100;
    [SerializeField]
    public ProgressBar HealthBar;

    private NavMeshAgent Agent;
    private float MaxHealth;

    public bool isImmortal = false;

    

    private void Awake()
    {
        MaxHealth = Health;
        Agent = GetComponent<NavMeshAgent>();
    }

    public void OnTakeDamage(int Damage)
    {
        if (!isImmortal)
        {
            Health -= Damage;
            Debug.Log(Health);
            HealthBar.SetProgress(Health / MaxHealth, 3);

            if (Health <= 0)
            {
                Debug.Log("PETDead");
                OnDied();
                Agent.enabled = false;
            }
        }
        
    }

    public void fullHpCheat()
    {
        isImmortal = !isImmortal;
    }

    private void OnDied()
    {
        Debug.Log("Dead");
        NotifyDead();
        Destroy(gameObject, 1f);
        Destroy(HealthBar.gameObject, 1f);
    }

    public void SetupHealthBar(Camera Camera)
    {
        
        if (HealthBar.TryGetComponent<FaceCamera>(out FaceCamera faceCamera))
        {
            Debug.Log("fjejfej");
            faceCamera.Camera = Camera;
        }
    }
}
