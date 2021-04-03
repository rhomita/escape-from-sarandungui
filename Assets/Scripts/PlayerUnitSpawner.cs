using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private List<Transform> _spawnPoints;
    
    [Header("Team")]
    [SerializeField] private Material _playerColor;

    private Team _team;

    private int _soldierCost = 100;
    private int _tankCost = 400;

    private static string NOT_ENOUGH_MONEY_STRING = "Not enough money to spawn unit.";

    void Start()
    {
        _team = new Team(0, _playerColor);
    }
    
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
    
    public void SpawnTank()
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

    public void SpawnSoldier()
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

    private Transform GetSpawnPoint()
    {
        int index = Random.Range(0, _spawnPoints.Count);
        return _spawnPoints[index];
    }
}
