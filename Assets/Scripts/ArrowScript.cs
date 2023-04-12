using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int damage = 35;
    BoxCollider arrowCollider;
    int shootableMask;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        arrowCollider = GetComponent<BoxCollider>();
        shootableMask = LayerMask.GetMask("Shootable");
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapBox(arrowCollider.bounds.center, arrowCollider.bounds.extents, arrowCollider.transform.rotation, shootableMask);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, hitCollider.transform.position);
                Destroy(gameObject);
            }
        }
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            Destroy(gameObject);
        }
    }
}
