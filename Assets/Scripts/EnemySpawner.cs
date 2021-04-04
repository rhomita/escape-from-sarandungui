using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : PlayerUnitSpawner
{
    private float _timeToSpawnTank = 15f;
    private float _timeToSpawnSoldier = 15f;

    private int _quantitySoldiers = 3;
    private int _quantityTanks = 1;

    void Start()
    {
        InvokeRepeating("SpawnTank", 1, _timeToSpawnTank);
        InvokeRepeating("SpawnSoldier", 1, _timeToSpawnSoldier);
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
}