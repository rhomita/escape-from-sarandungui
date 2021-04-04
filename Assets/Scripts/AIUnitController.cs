using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIUnitController : MonoBehaviour
{
    private Unit _unit;
    private Rocket _rocketTarget;

    private float _radiusToFindAttackableUnit = 20f;
    private float _timeToFindAttackableUnit = 3f;
    private float _timeToFindAttackableUnitCooldown = 0f;
    private LayerMask _attackableLayerMask;

    void Awake()
    {
        _unit = transform.GetComponent<Unit>();
        _unit.OnKilled += () => Destroy(this);
    }
    
    void Start()
    {
        _rocketTarget = GameManager.Instance.Rocket;
        _timeToFindAttackableUnit = 0f;
        _attackableLayerMask = PlayerUnitsManager.Instance.UnitsMask;
    }

    void Update()
    {
        if (_timeToFindAttackableUnitCooldown >= 0)
        {
            _timeToFindAttackableUnitCooldown -= Time.deltaTime;
        }

        FindAttackableUnit();
        
        if (!_unit.HasTargetSet)
        {
            _unit.SetAttackTarget(_rocketTarget);
        }
    }

    private void FindAttackableUnit()
    {
        if (_timeToFindAttackableUnitCooldown > 0) return;
        _timeToFindAttackableUnitCooldown = _timeToFindAttackableUnit;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radiusToFindAttackableUnit, _attackableLayerMask);
        if (colliders.Length <= 1) return;

        float maxDistance = float.MaxValue;
        Unit attackableUnit = null;
        foreach (Collider _collider in colliders)
        {
            if (_collider.transform == transform) continue; // Skip self.

            float distance = Vector3.SqrMagnitude(_collider.transform.position - transform.position);
            if (distance <= maxDistance && _collider.TryGetComponent(out Unit unit))
            {
                if (unit.Team.Number == _unit.Team.Number) continue;

                maxDistance = distance;
                attackableUnit = unit;
            }
        }

        if (attackableUnit != null)
        {
            _unit.SetAttackTarget(attackableUnit);
        }
    }
}
