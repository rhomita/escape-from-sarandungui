using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : PlayerUnitSpawner
{
    [SerializeField] private List<Transform> _spawnPoints;

    private float _timeToSpawnTank = 30f;
    private float _timeToSpawnSoldier = 20f;

    private int _quantitySoldiers = 3;
    private int _quantityTanks = 1;

    void Start()
    {
        InvokeRepeating("SpawnTank", _timeToSpawnTank, _timeToSpawnTank);
        InvokeRepeating("SpawnSoldier", _timeToSpawnSoldier, _timeToSpawnSoldier);
    }

    void Update()
    {
    }

    public override void SpawnTank()
    {
        for (int i = 0; i < _quantityTanks; i++)
        {
            Unit unit = _unitSpawner.SpawnTank(_team, GetSpawnPoint().position);
            unit.gameObject.AddComponent<AIUnitController>();
        }
    }

    public override void SpawnSoldier()
    {
        for (int i = 0; i < _quantitySoldiers; i++)
        {
            Unit unit = _unitSpawner.SpawnSoldier(_team, GetSpawnPoint().position);
            unit.gameObject.AddComponent<AIUnitController>();
        }
    }
    
    protected Transform GetSpawnPoint()
    {
        int index = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[index];
    }
}