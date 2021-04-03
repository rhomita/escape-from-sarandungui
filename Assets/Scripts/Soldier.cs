using UnityEngine;

public class Soldier : Unit
{
    [SerializeField] private Animator _animator;
    
    [Header("Bullet")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    
    private Ragdoll _ragdoll;

    protected float _attackSpeedDecrease;
    
    protected override void Awake()
    {
        _ragdoll = transform.GetComponent<Ragdoll>();
        _minAttackRange = 7f;
        _maxAttackRange = 24f;
        _angleToShot = 7;
        _attackSpeedDecrease = 2f;
        _maxHealth = 100f;
        
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _ragdoll.SetEnabled(false);
    }

    protected override void OnUpdate()
    {
        bool isShooting = false;
        if (_unitTarget != null)
        {
            Vector3 targetPosition = _unitTarget.transform.position;
            Vector3 directionToTarget = targetPosition - transform.position;

            directionToTarget.y = transform.position.y;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            bool inFront = angle < _angleToShot;
            Debug.Log(angle);

            float sqrDistance = Vector3.SqrMagnitude(directionToTarget);
            isShooting = sqrDistance < _maxAttackRange && inFront;
            
            if (sqrDistance < _minAttackRange)
            {
                RotateTowardsPosition(_unitTarget.transform.position);
                _navMeshAgent.isStopped = true;
            }
            else
            {
                _navMeshAgent.speed = _speed - _attackSpeedDecrease;
                SetDestination(_unitTarget.transform.position);
            }
            
            if (isShooting) Shoot();
        }
        
        bool isRunning = _navMeshAgent.velocity.magnitude > 0.1f;
        if (isRunning)
        {
            RotateTowardsPosition(_navMeshAgent.destination);
        }
        
        _animator.SetBool("isRunning", isRunning);
        _animator.SetBool("isShooting", isShooting);
    }

    protected override void OnShoot()
    {
        SimplePool.Spawn(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
    }

    protected override void Kill(Vector3 damageForce)
    {
        base.Kill(damageForce);
        _ragdoll.SetEnabled(true);
        _ragdoll.AddImpulse(damageForce);
    }

}