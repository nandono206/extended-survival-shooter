using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IDamageable
{
    public AttackRadius AttackRadius;
    public GameObject Player;
    public int Level;
    public Animator Animator;
    public BossMovement Movement;
    public NavMeshAgent Agent;
    public int Health = 500;
    public SkillScriptableObject[] Skills;
    public delegate void DeathEvent(Boss enemy);
    public DeathEvent OnDie;
    public GameObject bossHealthUI;
    public Slider bossHealthSlider;

    private Coroutine LookCoroutine;
    public const string ATTACK_TRIGGER = "Attack";

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        bossHealthUI = GameObject.Find("BossHealthUI");
        bossHealthSlider = GameObject.Find("BossHealthSlider").GetComponent<Slider>();
        AttackRadius.OnAttack += OnAttack;
        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i].UseTime = 0f;
            Skills[i].IsActivating = false;
        }
        bossHealthUI.SetActive(true);
        
    }

    private void Update()
    {
        //Debug.Log("On Update Boss");
        for (int i = 0; i < Skills.Length; i++)
        {
            if (Skills[i].CanUseSkill(this, Player, Level))
            {
                //Debug.Log("Can Shoot");
                Skills[i].UseSkill(this, Player);
            }
            else
            {
                //Debug.Log("Cannott");
            }
        }
        bossHealthSlider.value = gameObject.GetComponent<EnemyHealth>().currentHealth;
        
    }

    private void OnAttack(GameObject Target)
    {
        Animator.SetTrigger(ATTACK_TRIGGER);
        Debug.Log("iejige");

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.transform));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            transform.rotation = Quaternion.Euler(transform.rotation.x, targetRotation.eulerAngles.y, transform.rotation.z);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

   

    public void OnTakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            OnDie?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }


}
