using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private Unit _soldierPrefab;
    [SerializeField] private Unit _tankPrefab;

    [Header("Teams")]
    [SerializeField] private Material _enemyColor;
    
    void Update()
    {
        // TODO: USE POOL FOR UNITS.
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnTank(new Team(1, _enemyColor), transform.position);
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnSoldier(new Team(1, _enemyColor), transform.position);
        }
    }

    public Unit SpawnTank(Team team, Vector3 position)
    {
        Unit unit = SimplePool.Spawn(_tankPrefab.gameObject, position, transform.rotation).GetComponent<Unit>();
        unit.InitTeam(team);
        return unit;
    }

    public Unit SpawnSoldier(Team team, Vector3 position)
    {
        Unit unit = SimplePool.Spawn(_soldierPrefab.gameObject, position, transform.rotation).GetComponent<Unit>();
        unit.InitTeam(team);
        return unit;
    }
}
