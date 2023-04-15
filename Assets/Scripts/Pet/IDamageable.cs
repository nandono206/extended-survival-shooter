using UnityEngine;

public interface IDamageable
{
    void OnTakeDamage(int Damage);

    Transform GetTransform();
}
