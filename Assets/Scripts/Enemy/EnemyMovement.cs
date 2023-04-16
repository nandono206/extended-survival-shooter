using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class EnemyMovement : MonoBehaviour, PetObserver
{
    Transform player;

    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;


    GameObject pet;
    GameObject spawener;
    [SerializeField] PetSubject petSubject;
    UnityEngine.AI.NavMeshAgent nav;
    //FoxHealth foxHealth;


    private void Awake ()
    {
        // Peroleh game object dengan tag "Player"
        player = GameObject.FindGameObjectWithTag ("Player").transform;

        
        spawener = GameObject.FindGameObjectWithTag("Spawner");
        petSubject = spawener.GetComponent<Spawner>();
        if (petSubject != null)
        {

            petSubject.AddObserver(this);

        }


        // Peroleh reference component
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
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
        
        // Memindahkan posisi enemy
        
        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {

            

            nav.SetDestination(GetClosestTransform(player, pet));





        }
        else
        {
            // Menghentikan moving
            nav.enabled = false;
        }
    }

    private Vector3 GetClosestTransform(Transform transformA, GameObject transformB)
    {
        Vector3 targetPosition = Vector3.zero;
        float distanceA = Vector3.Distance(nav.transform.position, transformA.position);
        float distanceB = transformB != null ? Vector3.Distance(nav.transform.position, transformB.transform.position) : Mathf.Infinity;

        if (distanceA <= distanceB)
        {
            //Debug.Log("Chase Player");
            targetPosition = transformA.position;
            //if (transformB != null)
            //{
            //    targetPosition = transformA.transform.position;
            //}
            
        }
        else
        {
           //Debug.Log("Chase Pet");
            targetPosition = transformB.transform.position;
        }

        return targetPosition;
    }


    public void OnNotify(string petTag)
    {
        
        pet = GameObject.FindGameObjectWithTag(petTag);
    }

    public void OnNotifyDead()
    {
        pet = null;
    }
}
