using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitController : MonoBehaviour
{
    private Unit _unit;
    private Rocket _rocketTarget;

    private Unit _attackableUnit;
    private float _radiusToFindAttackableUnit = 20f;
    private float _timeToFindAttackableUnit = 5f;
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
        _attackableLayerMask = LayerMask.NameToLayer("Attackable");
    }

    void Update()
    {
        if (_timeToFindAttackableUnitCooldown >= 0)
        {
            _timeToFindAttackableUnitCooldown -= Time.deltaTime;
        }

        if (_attackableUnit == null)
        {
            _unit.SetAttackTarget(_rocketTarget);
        }
    }

    private void FindAttackableUnit()
    {
        if (_timeToFindAttackableUnitCooldown > 0) return;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radiusToFindAttackableUnit, _attackableLayerMask);
        Debug.Log(colliders);
        _timeToFindAttackableUnitCooldown = _timeToFindAttackableUnit;
    }
}
