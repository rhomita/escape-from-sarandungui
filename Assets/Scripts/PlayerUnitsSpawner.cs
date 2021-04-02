using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsSpawner : MonoBehaviour
{
    [SerializeField] private Unit _soldierPrefab;
    [SerializeField] private Unit _tankPrefab;
    
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
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(_soldierPrefab, transform.position, transform.rotation);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Unit unitCreated = Instantiate(_tankPrefab, transform.position, transform.rotation);
            PlayerUnitsManager.Instance.Register(unitCreated);
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(_tankPrefab, transform.position, transform.rotation);
        }
    }
}
