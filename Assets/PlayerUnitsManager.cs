using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsManager : MonoBehaviour
{
    public List<Unit> Units => _units;

    private List<Unit> _units;
    public static PlayerUnitsManager Instance { get; private set; }

    void Awake()
    {
        _units = new List<Unit>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(Unit unit)
    {
        if (_units.Contains(unit)) return;
        _units.Add(unit);
    }
    
    public void Deregister(Unit unit)
    {
        if (!_units.Contains(unit)) return;
        _units.Remove(unit);
    }
}
