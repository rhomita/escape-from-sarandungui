using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsSpawner : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private Unit _soldierPrefab;
    [SerializeField] private Unit _tankPrefab;

    [Header("Teams")]
    [SerializeField] private Material _playerColor;
    [SerializeField] private Material _enemyColor;
    
    void Start()
    {
        
    }

    void Update()
    {
        // TODO: USE POOL FOR UNITS.
        if (Input.GetKeyDown(KeyCode.R))
        {
            Unit unitCreated = Instantiate(_soldierPrefab, transform.position, transform.rotation);
            PlayerUnitsManager.Instance.Register(unitCreated);
            unitCreated.InitTeam(new Team(0, _playerColor));
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Unit unitCreated = Instantiate(_soldierPrefab, transform.position, transform.rotation);
            unitCreated.InitTeam(new Team(1, _enemyColor));
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Unit unitCreated = Instantiate(_tankPrefab, transform.position, transform.rotation);
            PlayerUnitsManager.Instance.Register(unitCreated);
            unitCreated.InitTeam(new Team(0, _playerColor));
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            Unit unitCreated = Instantiate(_tankPrefab, transform.position, transform.rotation);
            unitCreated.InitTeam(new Team(1, _enemyColor));
        }
    }
}
