using UnityEngine;

public class Bullet : Projectile
{
    private float _speed = 6f;
    private float _damageForce = 70f;
    
    protected override void OnInit()
    {
        _minDamage = 5;
        _maxDamage = 15;
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    protected override void OnUpdate()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!_initialized) return;
        if (collider.TryGetComponent(out Attackable attackable))
        {
            if (collider.TryGetComponent(out Unit unit))
            {
                if (unit.Team.Number == _team.Number) return;
            }
            int damage = GetDamage();
            Vector3 direction = collider.transform.position - transform.position;
            attackable.TakeDamage(damage, direction.normalized * _damageForce);
            SimplePool.Despawn(gameObject);
        }
    }
}
