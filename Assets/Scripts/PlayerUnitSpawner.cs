using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] protected UnitSpawner _unitSpawner;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] protected Team _team;

    private int _soldierCost = 100;
    private int _tankCost = 400;

    private static string NOT_ENOUGH_MONEY_STRING = "Not enough money to spawn unit.";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnSoldier();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnTank();
        }
    }
    
    public virtual void SpawnTank()
    {
        bool success = PlayerMoneyManager.Instance.Remove(_tankCost);
        if (!success)
        {
            UIManager.Instance.ShowMessagePopup(NOT_ENOUGH_MONEY_STRING);
            return;
        }
        Unit unit = _unitSpawner.SpawnTank(_team, GetSpawnPoint().position);
        PlayerUnitsManager.Instance.Register(unit);
    }

    public virtual void SpawnSoldier()
    {
        bool success = PlayerMoneyManager.Instance.Remove(_soldierCost);
        if (!success)
        {
            UIManager.Instance.ShowMessagePopup(NOT_ENOUGH_MONEY_STRING);
            return;
        }
        Unit unit = _unitSpawner.SpawnSoldier(_team, GetSpawnPoint().position);
        PlayerUnitsManager.Instance.Register(unit);
    }

    protected Transform GetSpawnPoint()
    {
        int index = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[index];
    }
}
