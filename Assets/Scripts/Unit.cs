using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [Header("Bullet")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    
    public delegate void OnKilledEvent();
    public OnKilledEvent OnKilled;
    public bool IsDead => _health <= 0;
    
    private NavMeshAgent _navMeshAgent;
    private Unit _unitTarget;

    private float _minAttackRange = 7f;
    private float _maxAttackRange = 24f;
    private float _angleToShot = 165;
    private float _speed;
    private float _attackSpeedDecrease = 2f;

    private float _attackCooldownTimer;
    private static float ATTACK_COOLDOWN_TIME = 1;
    
    // Stats
    private float _health;

    void Awake()
    {
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
        _maxAttackRange = Mathf.Pow(_maxAttackRange, 2);
        _minAttackRange = Mathf.Pow(_minAttackRange, 2);
    }

    void Start()
    {
        _speed = _navMeshAgent.speed;
    }
    
    private void OnEnable()
    {
        _health = 100;
        _attackCooldownTimer = 0;
    }

    void Update()
    {
        if (_attackCooldownTimer >= 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }

        if (_health <= 0)
        {
            return;
        }

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

    public void MoveTo(Vector3 position)
    {
        _navMeshAgent.speed = _speed;
        RemoveTarget();
        SetDestination(position);
    }

    public void SetAttackUnit(Unit unitTarget)
    {    
        RemoveTarget();
        _unitTarget = unitTarget;
        _unitTarget.OnKilled = OnTargetKilled;
        _navMeshAgent.isStopped = true;
    }

    public void RemoveTarget()
    {
        if (_unitTarget == null) return;
        _unitTarget.OnKilled -= OnTargetKilled;
        _unitTarget = null;
    }
    
    public void OnTargetKilled()
    {
        _unitTarget = null;
    }

    private void Shoot()
    {
        // TODO: ROTATE TO ATTACK.
        if (_attackCooldownTimer <= 0)
        {
            _attackCooldownTimer = ATTACK_COOLDOWN_TIME;
            SimplePool.Spawn(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        }
    }

    private void Kill()
    {
        OnKilled();
        enabled = false;
        Destroy(gameObject, 10);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;
        _health -= damage;
        if (IsDead)
        {
            Kill();
        }
    }

    private void SetDestination(Vector3 position)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(position);
    }

    private void RotateTowardsPosition(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _navMeshAgent.angularSpeed);
    }
}
