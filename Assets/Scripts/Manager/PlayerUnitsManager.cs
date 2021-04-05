using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsManager : MonoBehaviour
{
    [SerializeField] private LayerMask _unitsMask;
    public LayerMask UnitsMask => _unitsMask;
    public List<Unit> Units => _units;

    private List<Unit> _units;
    public static PlayerUnitsManager Instance { get; private set; }

    void Awake()
    {
        _units = new List<Unit>();
        Instance = this;
    }

    public void Register(Unit unit)
    {
        if (_units.Contains(unit)) return;
        _units.Add(unit);
        unit.OnKilled += () => Deregister(unit);
    }
    
    private void Deregister(Unit unit)
    {
        if (!_units.Contains(unit)) return;
        _units.Remove(unit);
    }
}
