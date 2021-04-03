﻿using System;
using UnityEngine;

public class Soldier : Unit
{
    [SerializeField] private Animator _animator;
    
    [Header("Bullet")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    [Header("Mesh")] 
    [SerializeField] private SkinnedMeshRenderer _bodyMesh;
    [SerializeField] private MeshRenderer _weaponMesh;
    
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
        Bullet bullet = SimplePool.Spawn(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.Init(this);
    }

    protected override void Kill(Vector3 damageForce)
    {
        base.Kill(damageForce);
        _ragdoll.SetEnabled(true);
        _ragdoll.AddImpulse(damageForce);
    }

}