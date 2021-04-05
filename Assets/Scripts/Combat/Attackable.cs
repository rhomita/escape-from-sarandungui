using UnityEngine;

public abstract class Attackable : MonoBehaviour
{
    public delegate void OnKilledEvent();
    public OnKilledEvent OnKilled;
    public delegate void OnTakeDamageEvent();
    public OnTakeDamageEvent OnTakeDamage;
    public abstract bool IsDead();
    public abstract void TakeDamage(int damage, Vector3 damageForce, Attackable attacker = null);
    
    protected virtual void Kill(Vector3 damageForce)
    {
        OnKilled?.Invoke();
    }
}