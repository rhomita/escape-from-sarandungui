using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] private MeshRenderer _renderer;

    private float _speed = 13f;
    private float _damageForce = 70f;
    private float _damageUpForce = 10f;
    
    protected override void OnInit()
    {
        _minDamage = 5;
        _maxDamage = 15;
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    public override void Init(Unit owner)
    {
        base.Init(owner);
        _renderer.material = _owner.Team.ProjectileMaterial;
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
                if (unit.Team.Number == _owner.Team.Number) return;
                ParticlesManager.Instance.Spawn("blood", collider.transform.position);
            }
            int damage = GetDamage();
            Vector3 damageForce = (collider.transform.position - transform.position).normalized * _damageForce +
                                  Vector3.up * _damageUpForce;
            attackable.TakeDamage(damage, damageForce, _owner);
            SimplePool.Despawn(gameObject);
        }
    }
}
