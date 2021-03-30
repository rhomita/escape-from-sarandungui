using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    
    void Start()
    {
        
    }

    void Update()
    {
        // TODO: USE POOL FOR UNITS.
        if (Input.GetKeyDown(KeyCode.R))
        {
            Unit unitCreated = Instantiate(_unitPrefab, transform.position, transform.rotation);
            PlayerUnitsManager.Instance.Register(unitCreated);
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(_unitPrefab, transform.position, transform.rotation);
        }
    }
}
