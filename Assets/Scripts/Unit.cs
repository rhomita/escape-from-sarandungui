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
    
    private NavMeshAgent _navMeshAgent;
    private Unit _unitTarget;

    private float _minAttackRange = 7f;
    private float _maxAttackRange = 15f;

    private bool _isDead = false;
    private float attackCooldownTimer;
    private static float ATTACK_COOLDOWN = 1;
    
    // Stats
    private float _health;
    
    void Awake()
    {
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
        _maxAttackRange = Mathf.Pow(_maxAttackRange, 2);
        _minAttackRange = Mathf.Pow(_minAttackRange, 2);
    }

    private void OnEnable()
    {
        _health = 100;
        attackCooldownTimer = 0;
    }

    void Update()
    {
        if (attackCooldownTimer >= 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        if (_health <= 0)
        {
            if (_isDead) return;
            Kill();
            return;
        }
        
        bool isShooting = false;
        if (_unitTarget != null)
        {
            Vector3 targetPosition = _unitTarget.transform.position;
            float sqrDistance = Vector3.SqrMagnitude(targetPosition - transform.position);
            isShooting = sqrDistance < _maxAttackRange;
            if (sqrDistance < _minAttackRange)
            {
                _navMeshAgent.isStopped = true;
            }
            else
            {
                SetDestination(_unitTarget.transform.position);
            }
            
            if (isShooting) Shoot();
        }
        
        bool isRunning = _navMeshAgent.velocity.magnitude > 0.1f;
        _animator.SetBool("isRunning", isRunning);
        _animator.SetBool("isShooting", isShooting);
    }

    public void MoveTo(Vector3 position)
    {
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
    }
    
    public void OnTargetKilled()
    {
        _unitTarget = null;
    }

    private void Shoot()
    {
        // TODO: ROTATE TO ATTACK.
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = ATTACK_COOLDOWN;
            SimplePool.Spawn(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        }
    }

    private void Kill()
    {
        _isDead = true;
        OnKilled();
        this.enabled = false;
        Destroy(gameObject, 10);
    }

    private void SetDestination(Vector3 position)
    {
        // Vector3 direction = (position - transform.position).normalized;
        // direction.y = 0;
        // Quaternion rotation = Quaternion.LookRotation(direction);
        // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _navMeshAgent.angularSpeed);
        
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(position);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            
            bullet.OnUnitHit();
        }
    }
}
