using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    [SerializeField] private Transform _cannon;
    [SerializeField] private Transform _cannonSpawnPoint;
    [SerializeField] private Missile _missilePrefab;

    private float _minAngleToMoveForward = 9f;
    private float _cannonRotateSpeed = 2f;
    
    protected override void Awake()
    {
        _maxAttackRange = 40f;
        _angleToShot = 3f;
        
        base.Awake();
    }
    
    protected override void OnUpdate()
    {
        bool hasTargetSet = _unitTarget != null;
        Vector3 destination = hasTargetSet ? _unitTarget.transform.position : _navMeshAgent.destination;
        Vector3 directionToTarget = destination - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        bool shouldRotateBeforeMove = angle > _minAngleToMoveForward;

        _navMeshAgent.isStopped = shouldRotateBeforeMove;

        if (hasTargetSet)
        {
            float sqrDistance = Vector3.SqrMagnitude(directionToTarget);
            if (sqrDistance < _maxAttackRange)
            {
                float cannonAngle = Vector3.Angle(_cannon.forward, directionToTarget);
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
        Debug.Log("Shoot!");
    }

    private void RotateCannon(Vector3 position)
    {
        Vector3 direction = (position - _cannon.position).normalized;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        _cannon.rotation = Quaternion.Slerp(_cannon.rotation, rotation, Time.deltaTime * _cannonRotateSpeed);
    }
}
