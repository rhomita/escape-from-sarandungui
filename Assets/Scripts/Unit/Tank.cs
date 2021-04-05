using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Tank : Unit
{
    [SerializeField] private Transform _cannon;
    [SerializeField] private Transform _cannonSpawnPoint;
    [SerializeField] private GameObject _missilePrefab;

    [Header("Mesh")] 
    [SerializeField] private List<MeshRenderer> _cannonMeshes;
    [SerializeField] private MeshRenderer _tankMesh;

    [Header("SFX")] 
    [SerializeField] private SoundEffect _shootingSound;
    [SerializeField] private SoundEffect _movingSound;
    [SerializeField] private SoundEffect _rotateCannonSound;
    [SerializeField] private SoundEffect _impactSound;

    [Header("Particles")] [SerializeField] private GameObject _fireParticles;
    
    private Animator _animator;
    
    private float _minAngleToMoveForward = 15f;
    private float _cannonRotateSpeed = 4f;

    public override void InitTeam(Team team)
    {
        _team = team;
        Material[] materials = _tankMesh.materials;
        materials[3] = _team.Material;
        _tankMesh.materials = materials;

        foreach (MeshRenderer meshRenderer in _cannonMeshes)
        {
            materials = meshRenderer.materials;
            materials[0] = _team.Material;
            meshRenderer.materials = materials;
        }
    }
    
    protected override void OnEnable()
    {
        _fireParticles.SetActive(false);
        base.OnEnable();
    }

    protected override void Kill(Vector3 damageForce)
    {
        base.Kill(damageForce);
        ParticlesManager.Instance.Spawn("explosion", transform.position);
        _fireParticles.SetActive(true);
    }
    
    protected override void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _maxAttackRange = 40f;
        _angleToShot = 1.5f;
        _maxHealth = 500f;
        _attackCooldownTime = 2f;
        
        OnTakeDamage += () => _impactSound.Play();

        base.Awake();
    }

    protected override void OnUpdate()
    {
        bool hasTargetSet = _attackTarget != null;
        Vector3 destination = hasTargetSet ? _attackTarget.transform.position : _navMeshAgent.destination;
        destination.y = 0;
        Vector3 directionToTarget = destination - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        bool shouldRotateBeforeMove = angle > _minAngleToMoveForward;

        _navMeshAgent.isStopped = shouldRotateBeforeMove;
        if (!_navMeshAgent.isStopped)
        {
            _rotateCannonSound.Stop();
            _movingSound.Play();
        }
        else
        {
            _movingSound.Stop();
        }

        if (hasTargetSet)
        {
            float sqrDistance = Vector3.SqrMagnitude(directionToTarget);
            if (sqrDistance < _maxAttackRange)
            {
                _navMeshAgent.isStopped = true;
                destination.y = _cannon.position.y;
                Vector3 directionToTargetFromCanon = destination - _cannon.position;
                float cannonAngle = Vector3.Angle(_cannon.forward, directionToTargetFromCanon);
                if (cannonAngle < _angleToShot)
                {
                    Shoot();
                }
                else
                {
                    RotateCannon(destination);
                }

                return;
            }
        }
        else
        {
            ResetCannonRotation();
        }

        if (!_isMovingToASelectedPosition && !hasTargetSet) return;
        
        if (shouldRotateBeforeMove)
        {
            RotateTowardsPosition(destination);
        }
        else
        {
            RotateTowardsPosition(destination);
            SetDestination(destination);
        }
    }

    protected override void OnShoot()
    {
        _movingSound.Stop();
        _rotateCannonSound.Stop();
        _animator.SetTrigger("Shoot");
        _shootingSound.Play();
        Missile missile = SimplePool.Spawn(_missilePrefab, _cannonSpawnPoint.position, _cannonSpawnPoint.rotation).GetComponent<Missile>();
        missile.Init(this);
    }

    private void RotateCannon(Vector3 position)
    {
        _rotateCannonSound.Play();
        Vector3 direction = (position - _cannon.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        _cannon.rotation =
            Quaternion.Lerp(_cannon.rotation, rotation, Time.deltaTime * _cannonRotateSpeed);
    }

    private void ResetCannonRotation()
    {
        _cannon.rotation = Quaternion.Slerp(_cannon.rotation, transform.rotation, Time.deltaTime * _cannonRotateSpeed);
    }
}