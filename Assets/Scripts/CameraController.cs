using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
    
public class CameraController : MonoBehaviour
{
    [SerializeField] private RectTransform _selectionPanel;

    private float _raycastDistance = 300;
    private Camera _cam;
    private HashSet<Unit> _selectedUnits;
    
    void Start()
    {
        _selectedUnits = new HashSet<Unit>();
        _selectionPanel.gameObject.SetActive(false);
        _cam = transform.GetComponent<Camera>();
    }

    void Update()
    {
        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            StartClick();
        }
        
        if (Input.GetMouseButton(0))
        {
            KeepPressedClick();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRightClick();
        }
    }

    void StartClick()
    {
        _selectionPanel.gameObject.SetActive(true);
        _selectionPanel.position = Input.mousePosition;
        foreach (Unit unit in _selectedUnits)
        {
            unit.DeSelect();
        }
        _selectedUnits.Clear();
    }

    void KeepPressedClick()
    {
        float xSize = Input.mousePosition.x - _selectionPanel.position.x;
        float ySize = _selectionPanel.position.y - Input.mousePosition.y;
        _selectionPanel.sizeDelta = new Vector2(xSize, ySize);
    }

    void ReleaseClick()
    {
        _selectionPanel.gameObject.SetActive(false);

        Vector3 startPosition = _selectionPanel.position;
        Vector3 endPosition = Input.mousePosition;

        foreach (Unit unit in PlayerUnitsManager.Instance.Units)
        {
            Vector3 unitPosition = _cam.WorldToScreenPoint(unit.transform.position);
            if (unitPosition.x > startPosition.x
                && unitPosition.x < endPosition.x
                && unitPosition.y > endPosition.y
                && unitPosition.y < startPosition.y)
            {
                _selectedUnits.Add(unit);
            }
        }
        
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit _hit, _raycastDistance))
        {
            if (_hit.collider.TryGetComponent(out Unit _unit))
            {
                if (PlayerUnitsManager.Instance.Units.Contains(_unit))
                {
                    _selectedUnits.Add(_unit);
                }
            }
        }
        
        foreach (Unit unit in _selectedUnits)
        {
            unit.Select();
        }
    }

    void OnRightClick()
    {
        if (_selectedUnits.Count == 0) return;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray, out RaycastHit _hit, _raycastDistance))
        {
            if (_hit.collider.TryGetComponent(out Unit _unit))
            {
                if (!PlayerUnitsManager.Instance.Units.Contains(_unit))
                {
                    foreach (Unit unit in _selectedUnits)
                    {
                        unit.SetAttackUnit(_unit);
                    }
                }
                return;
            }
                
                
            Vector3 endPosition = _hit.point;
            bool blocked = NavMesh.Raycast(transform.position, endPosition, out NavMeshHit hit, NavMesh.AllAreas);
            if (blocked) return; // Cannot move there
            foreach (Unit unit in _selectedUnits)
            {
                unit.MoveTo(endPosition);
            }
        }    
    }
}
