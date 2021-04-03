using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private GameObject _damagePopupPrefab;
    [SerializeField] private GameObject _selection;
    [SerializeField] private FloatingHealthBar floatingHealthBar;
    
    public delegate void OnKilledEvent();
    public OnKilledEvent OnKilled;
    public bool IsDead => _health <= 0;
    public Team Team => _team;
    
    protected Collider _collider;
    protected NavMeshAgent _navMeshAgent;
    protected Unit _unitTarget;

    protected float _maxHealth;
    protected float _minAttackRange;
    protected float _maxAttackRange;
    protected float _angleToShot;
    protected float _speed;
    protected Team _team;
    protected float _timeToDespawnAfterKilled = 8f;

    private float _attackCooldownTimer;
    private static float ATTACK_COOLDOWN_TIME = 1;
    
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
        _unitTarget = null;
        _selection.SetActive(false);
        floatingHealthBar.SetMaxHealth(_health);
        floatingHealthBar.SetHealth(_health);
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
        if (IsDead) return;
        _navMeshAgent.speed = _speed;
        RemoveTarget();
        SetDestination(position);
    }

    public void SetAttackUnit(Unit unitTarget)
    {
        if (IsDead) return;
        if (unitTarget.IsDead) return;
        RemoveTarget();
        _unitTarget = unitTarget;
        _unitTarget.OnKilled += OnTargetKilled;
        _navMeshAgent.isStopped = true;
    }

    public void RemoveTarget()
    {
        if (IsDead) return;
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

    protected virtual void Kill(Vector3 damageForce)
    {
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
    
    public void TakeDamage(int damage, Vector3 damageForce)
    {
        if (IsDead) return;

        DamagePopup damagePopup = SimplePool.Spawn(_damagePopupPrefab, Vector3.zero, Quaternion.identity).GetComponent<DamagePopup>();
        damagePopup.Show(transform.position, damage);
        
        _health -= damage;
        floatingHealthBar.SetHealth(_health);
        if (IsDead)
        {
            Kill(damageForce);
        }
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
}
