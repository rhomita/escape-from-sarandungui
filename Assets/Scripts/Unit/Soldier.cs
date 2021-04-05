using System;
using UnityEngine;
using Util;

public class Soldier : Unit
{
    [SerializeField] private Animator _animator;

    [Header("Bullet")] 
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    [Header("Mesh")] 
    [SerializeField] private SkinnedMeshRenderer _bodyMesh;
    [SerializeField] private MeshRenderer _weaponMesh;

    [Header("SFX")] 
    [SerializeField] private SoundEffect _footstepsSound;
    [SerializeField] private SoundEffect _shootSound;
    [SerializeField] private SoundEffect _dieSound;
    [SerializeField] private SoundEffect _screamSound;
    [SerializeField] private SoundEffect _impactSound;

    private Ragdoll _ragdoll;

    protected float _attackSpeedDecrease;

    public override void InitTeam(Team team)
    {
        _team = team;
        Material[] materials = _bodyMesh.materials;
        materials[2] = _team.Material;
        materials[3] = _team.Material;
        _bodyMesh.materials = materials;

        materials = _weaponMesh.materials;
        materials[2] = _team.Material;
        _weaponMesh.materials = materials;
    }

    protected override void Awake()
    {
        _ragdoll = transform.GetComponent<Ragdoll>();
        _minAttackRange = 7f;
        _maxAttackRange = 24f;
        _angleToShot = 10f;
        _attackSpeedDecrease = 2f;
        _maxHealth = 100f;
        _attackCooldownTime = 1;

        OnTakeDamage += () => _impactSound.Play();

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
        if (_attackTarget != null)
        {
            Vector3 targetPosition = _attackTarget.transform.position;
            Vector3 directionToTarget = targetPosition - transform.position;

            directionToTarget.y = transform.position.y;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            bool inFront = angle < _angleToShot;

            float sqrDistance = Vector3.SqrMagnitude(directionToTarget);
            isShooting = sqrDistance < _maxAttackRange && inFront;

            if (sqrDistance < _minAttackRange)
            {
                RotateTowardsPosition(_attackTarget.transform.position);
                _navMeshAgent.isStopped = true;
            }
            else
            {
                _navMeshAgent.speed = isShooting ? _speed - _attackSpeedDecrease : _speed;
                SetDestination(targetPosition);
            }

            if (isShooting) Shoot();
        }

        bool isRunning = _navMeshAgent.velocity.magnitude > 0.1f;
        if (isRunning)
        {
            _footstepsSound.Play();
            RotateTowardsPosition(_navMeshAgent.destination);
        }

        _animator.SetBool("isRunning", isRunning);
        _animator.SetBool("isShooting", isShooting);
    }

    protected override void OnShoot()
    {
        _shootSound.Play();
        Bullet bullet = SimplePool.Spawn(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation)
            .GetComponent<Bullet>();
        bullet.Init(this);
    }

    protected override void Kill(Vector3 damageForce)
    {
        base.Kill(damageForce);
        if (damageForce.magnitude > 50f)
        {
            _screamSound.Play();
        }
        else
        {
            _dieSound.Play();
        }
        _ragdoll.SetEnabled(true);
        _ragdoll.AddImpulse(damageForce);
    }
}