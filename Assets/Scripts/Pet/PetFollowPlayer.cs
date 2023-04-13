using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PetFollowPlayer : MonoBehaviour
{
    
    public string tagToAvoid = "Enemy"; // tag of the objects to avoid
    public float speed = 5f; // speed at which the object moves
    public float detectionRange = 1.2f; // distance to detect objects to avoid
    public float avoidDistance = 1.5f;
    public float followSpeed = 5f;
    public float radius = 1f;

    Transform target;
    NavMeshAgent nav;
    Vector3 previousPos;
    Animator anim;
    float movementThreshold = 0.0f;
    

    void Awake()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<PetController>().Health > 0 && target.GetComponent<PlayerHealth>().currentHealth>0)
        {
            Vector3 playerPos = target.position;

            Vector3 center = playerPos;
            Vector3 direction = transform.position - center;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;
            Vector3 circleCenter = center + perpendicular * radius;

            Vector3 followDirection = new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z);

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
            Vector3 avoidDirection = Vector3.zero;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(tagToAvoid))
                {
                    Vector3 awayFromObj = transform.position - collider.transform.position;
                    avoidDirection += new Vector3(awayFromObj.x, 0, awayFromObj.z).normalized;
                }
            }
            //Debug.Log(avoidDirection);

            // move the object towards the target object and away from the objects to avoid
            Vector3 movement = (followDirection + avoidDirection).normalized * followSpeed;
            movement.y = 0f; // ignore the y-axis component
            nav.SetDestination(transform.position + movement);

        }

        else
        {
            nav.enabled = false;
        }





        if (anim != null )
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
}
