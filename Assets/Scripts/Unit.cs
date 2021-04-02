using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    public delegate void OnKilledEvent();
    public OnKilledEvent OnKilled;
    public bool IsDead => _health <= 0;
    
    protected Collider _collider;
    protected NavMeshAgent _navMeshAgent;
    protected Unit _unitTarget;

    protected float _minAttackRange;
    protected float _maxAttackRange;
    protected float _angleToShot;
    protected float _speed;

    private float _attackCooldownTimer;
    private static float ATTACK_COOLDOWN_TIME = 1;
    
    // Stats
    private float _health;

    protected abstract void OnUpdate();
    protected abstract void OnShoot();

    protected virtual void Awake()
    {
        _collider = transform.GetComponent<Collider>();
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
        _maxAttackRange = Mathf.Pow(_maxAttackRange, 2);
        _minAttackRange = Mathf.Pow(_minAttackRange, 2);
    }

    void Start()
    {
        _speed = _navMeshAgent.speed;
    }
    
    protected virtual void OnEnable()
    {
        _health = 100;
        _attackCooldownTimer = 0;
        _collider.enabled = true;
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
        
        OnUpdate();
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

    protected void Shoot()
    {
        if (_attackCooldownTimer <= 0)
        {
            _attackCooldownTimer = ATTACK_COOLDOWN_TIME;
            OnShoot();
        }
    }

    protected virtual void Kill()
    {
        OnKilled();
        enabled = false;
        _collider.enabled = false;
        Destroy(gameObject, 10);
    }

    protected void SetDestination(Vector3 position)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(position);
    }

    protected void RotateTowardsPosition(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _navMeshAgent.angularSpeed);
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
}
