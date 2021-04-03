using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    [SerializeField] private Transform _cannon;
    [SerializeField] private Transform _cannonSpawnPoint;
    [SerializeField] private GameObject _missilePrefab;

    private float _minAngleToMoveForward = 9f;
    private float _cannonRotateSpeed = 4f;

    protected override void Awake()
    {
        _maxAttackRange = 40f;
        _angleToShot = 1.5f;
        _maxHealth = 400f;

        base.Awake();
    }

    protected override void OnUpdate()
    {
        bool hasTargetSet = _unitTarget != null;
        Vector3 destination = hasTargetSet ? _unitTarget.transform.position : _navMeshAgent.destination;
        destination.y = 0;
        Vector3 directionToTarget = destination - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        bool shouldRotateBeforeMove = angle > _minAngleToMoveForward;

        _navMeshAgent.isStopped = shouldRotateBeforeMove;

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

        if (shouldRotateBeforeMove)
        {
            RotateTowardsPosition(destination);
        }
        else
        {
            SetDestination(destination);
        }
    }

    protected override void OnShoot()
    {
        SimplePool.Spawn(_missilePrefab, _cannonSpawnPoint.position, _cannonSpawnPoint.rotation);
    }

    private void RotateCannon(Vector3 position)
    {
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