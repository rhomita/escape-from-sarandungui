using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : Attackable
{
    [SerializeField] private GameObject _damagePopupPrefab;
    [SerializeField] private GameObject _selection;
    [SerializeField] private FloatingHealthBar floatingHealthBar;
    
    public Team Team => _team;
    public bool HasTargetSet => _attackTarget != null;
    
    protected Collider _collider;
    protected NavMeshAgent _navMeshAgent;
    protected Attackable _attackTarget;

    protected float _maxHealth;
    protected float _minAttackRange;
    protected float _maxAttackRange;
    protected float _angleToShot;
    protected float _speed;
    protected Team _team;
    protected float _timeToDespawnAfterKilled = 8f;
    protected float _attackCooldownTime = 1;

    private float _attackCooldownTimer;
    
    // Stats
    private float _health;

    protected abstract void OnUpdate();
    protected abstract void OnShoot();
    public abstract void InitTeam(Team team);

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
        _health = _maxHealth;
        _attackCooldownTimer = 0;
        _collider.enabled = true;
        _navMeshAgent.enabled = true;
        _attackTarget = null;
        _selection.SetActive(false);
        floatingHealthBar.SetMaxHealth(_health);
        floatingHealthBar.SetHealth(_health);
        _navMeshAgent.SetDestination(transform.position + transform.forward * -2);
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
        if (IsDead()) return;

        _navMeshAgent.speed = _speed;
        RemoveTarget();
        SetDestination(position);
    }

    public void SetAttackTarget(Attackable attackTarget)
    {
        if (IsDead() || attackTarget.IsDead()) return;

        RemoveTarget();
        _attackTarget = attackTarget;
        _attackTarget.OnKilled += OnTargetKilled;
    }

    public void RemoveTarget()
    {
        if (IsDead()) return;
        if (_attackTarget == null) return;
        _attackTarget.OnKilled -= OnTargetKilled;
        _attackTarget = null;
    }
    
    public void OnTargetKilled()
    {
        _attackTarget = null;
    }

    protected void Shoot()
    {
        if (_attackCooldownTimer <= 0)
        {
            _attackCooldownTimer = _attackCooldownTime;
            OnShoot();
        }
    }

    protected override void Kill(Vector3 damageForce)
    {
        base.Kill(damageForce);
        OnKilled?.Invoke();
        _collider.enabled = false;
        _navMeshAgent.enabled = false;
        DeSelect();
        StartCoroutine(DeSpawn());
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
        if (direction.Equals(Vector3.zero)) return;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _navMeshAgent.angularSpeed);
    }
    
    public override void TakeDamage(int damage, Vector3 damageForce)
    {
        if (IsDead()) return;

        DamagePopup damagePopup = SimplePool.Spawn(_damagePopupPrefab, Vector3.zero, Quaternion.identity).GetComponent<DamagePopup>();
        damagePopup.Show(transform.position, damage);
        
        _health -= damage;
        floatingHealthBar.SetHealth(_health);
        if (IsDead())
        {
            Kill(damageForce);
            return;
        }
        OnTakeDamage?.Invoke();
    }

    public void Select()
    {
        _selection.SetActive(true);
    }

    public void DeSelect()
    {
        _selection.SetActive(false);   
    }
    
    private IEnumerator DeSpawn()
    {
        yield return new WaitForSeconds(_timeToDespawnAfterKilled);
        SimplePool.Despawn(this.gameObject);
    }

    public override bool IsDead()
    {
        return _health <= 0;
    }
}
