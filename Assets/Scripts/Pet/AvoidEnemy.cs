using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidEnemy : MonoBehaviour
{
    public string tagToAvoid = "Enemy"; // tag of the objects to avoid
    public float speed = 5f; // speed at which the object moves
    public float detectionRange = 10f; // distance to detect objects to avoid

    private void Update()
    {
        // get all colliders within the detection range and with the specified tag
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Vector3 direction = Vector3.zero;

        // loop through each collider and calculate the direction to move away
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tagToAvoid))
            {
                Vector3 awayFromObj = transform.position - collider.transform.position;
                awayFromObj.y = 0;
                direction += awayFromObj.normalized;
            }
        }

        // move the object away from the objects to avoid
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
