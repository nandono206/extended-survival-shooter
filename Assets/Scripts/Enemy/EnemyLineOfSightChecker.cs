using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(GameObject player);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(GameObject player);
    public LoseSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
       // GameObject player;
        if (other.CompareTag("Player"))
        {
            Debug.Log("player in sight");
            if (!CheckLineOfSight(other.gameObject))
            {
                Debug.Log("plfefwesfjgofhesghy");
                CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //GameObject player;
        if (other.CompareTag("Player"))
        {
            OnLoseSight?.Invoke(other.gameObject);
            if (CheckForLineOfSightCoroutine != null)
            {
                StopCoroutine(CheckForLineOfSightCoroutine);
            }
        }
    }

    private bool CheckLineOfSight(GameObject player)
    {
        Vector3 Direction = (player.transform.position - transform.position).normalized;
        float DotProduct = Vector3.Dot(transform.forward, Direction);
        if (DotProduct >= Mathf.Cos(FieldOfView))
        {
            RaycastHit Hit;

            if (Physics.Raycast(transform.position, Direction, out Hit, Collider.radius, LineOfSightLayers))
            {
                if (Hit.transform.GetComponent<PlayerMovement>() != null)
                {
                    OnGainSight?.Invoke(player);
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(GameObject player)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.1f);

        while (!CheckLineOfSight(player))
        {
            yield return Wait;
        }
    }
}
