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
        _angleToShot = 165;
        _attackSpeedDecrease = 2f;
        
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
            float sqrDistance = Vector3.SqrMagnitude(directionToTarget);
            
            float angle = Vector3.Angle(transform.forward, targetPosition - directionToTarget);
            bool inFront = angle > _angleToShot;
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

    protected override void Kill()
    {
        base.Kill();
        _ragdoll.SetEnabled(true);
    }
    
}