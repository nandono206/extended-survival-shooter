using UnityEngine;

public class SkillScriptableObject : ScriptableObject
{
    public float Cooldown = 1f;
    public int Damage = 5;
    public int UnlockLevel = 1;

    public bool IsActivating = false;

    public float UseTime;

    public virtual SkillScriptableObject ScaleUpForLevel(ScalingScriptableObject Scaling, int Level)
    {
        SkillScriptableObject scaledSkill = CreateInstance<SkillScriptableObject>();

        ScaleUpBaseValuesForLevel(scaledSkill, Scaling, Level);

        return scaledSkill;
    }

    protected virtual void ScaleUpBaseValuesForLevel(SkillScriptableObject Instance, ScalingScriptableObject Scaling, int Level)
    {
        Instance.name = name;

        Instance.Cooldown = Cooldown;
        Instance.Damage = Damage + Mathf.FloorToInt(Scaling.DamageCurve.Evaluate(Level));
        Instance.UnlockLevel = UnlockLevel;
    }

    public virtual void UseSkill(Boss Enemy, GameObject Player)
    {
        IsActivating = true;
    }

    public virtual bool CanUseSkill(Boss Enemy, GameObject Player, int Level)
    {
        Debug.Log("Cooldown Time: " + Cooldown.ToString());
        Debug.Log("use time: " + UseTime.ToString());
        Debug.Log("Current time: " + Time.time.ToString());
        Debug.Log("");
        if (IsActivating)
        {
            Debug.Log("IS ACTIVATING");
        }
        if (UseTime + Cooldown < Time.time)
        {
            Debug.Log("Cooldown done");
        }
        return !IsActivating
            && UseTime + Cooldown < Time.time;
    }

    protected void DisableEnemyMovement(Boss Enemy)
    {
        Enemy.enabled = false;
        Enemy.Agent.enabled = false;
        Enemy.Movement.enabled = false;
    }

    protected void EnableEnemyMovement(Boss Enemy)
    {
        Enemy.enabled = true;
        Enemy.Movement.enabled = true;
        Enemy.Agent.enabled = true;
    }
}
