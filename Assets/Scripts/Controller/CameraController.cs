using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private RectTransform _selectionPanel;

    private float _raycastDistance = 300;
    private Camera _cam;
    private HashSet<Unit> _selectedUnits { get; } = new HashSet<Unit>();
    private HashSet<Vector3> _points { get; } = new HashSet<Vector3>();
    public Vector3 originPoint = new Vector3(0, 0, 0);

    private void Start()
    {
        _selectionPanel.gameObject.SetActive(false);
        _cam = transform.GetComponent<Camera>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsAndroid)
        {
            if (Input.touchCount >= 2)
            {
                return;
            }

            if (GameManager.Instance.AndroidManager.SelectMode == SelectMode.SELECT)
            {
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
            }
            else
            {
                ReleaseClick();
                if (Input.GetMouseButtonDown(0))
                {
                    OnRightClick();
                }
            }
        }
        else
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
    }

    private void StartClick()
    {
        _selectionPanel.sizeDelta = Vector2.zero;
        _selectionPanel.gameObject.SetActive(true);
        _selectionPanel.position = Input.mousePosition;
        originPoint = Input.mousePosition;

        if (IsPointerOverGameObject())
        {
            return;
        }

        foreach (Unit unit in _selectedUnits)
        {
            unit.DeSelect();
        }

        _selectedUnits.Clear();
    }

    private void KeepPressedClick()
    {
        float xSize = Input.mousePosition.x - originPoint.x;
        float ySize = originPoint.y - Input.mousePosition.y;
        if (xSize < 0f)
        {
            _selectionPanel.position = new Vector3(Input.mousePosition.x, _selectionPanel.position.y,
                _selectionPanel.position.z);
        }

        if (ySize < 0f)
        {
            _selectionPanel.position = new Vector3(_selectionPanel.position.x, Input.mousePosition.y,
                _selectionPanel.position.z);
        }

        xSize = Mathf.Abs(xSize);
        ySize = Mathf.Abs(ySize);
        _selectionPanel.sizeDelta = new Vector2(xSize, ySize);
    }

    private void ReleaseClick()
    {
        KeepPressedClick();
        _selectionPanel.gameObject.SetActive(false);
        Vector3 mouse = Input.mousePosition;
        Vector3 startPosition = _selectionPanel.position;
        Vector3 endPosition = mouse;

        if (_selectionPanel.position.x == mouse.x)
        {
            endPosition = new Vector3(originPoint.x, mouse.y, mouse.z);
        }

        if (_selectionPanel.position.y == mouse.y)
        {
            endPosition = new Vector3(mouse.x, originPoint.y, mouse.z);
        }

        if (_selectionPanel.position == mouse)
        {
            endPosition = originPoint;
        }

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

    private void OnRightClick()
    {
        if (_selectedUnits.Count == 0) return;

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit _hit, _raycastDistance))
        {
            if (_hit.collider.TryGetComponent(out Unit _unit))
            {
                if (!PlayerUnitsManager.Instance.Units.Contains(_unit))
                {
                    foreach (Unit unit in _selectedUnits)
                    {
                        unit.SetAttackTarget(_unit);
                    }
                }

                return;
            }

            Vector3 endPosition = _hit.point;
            bool blocked = NavMesh.Raycast(transform.position, endPosition, out NavMeshHit hit, NavMesh.AllAreas);
            if (blocked) return; // Cannot move there

            _points.Clear();
            Vector3[] points = GetUnitTrainingPoints(endPosition, _selectedUnits.Count);

            int index = 0;
            foreach (Unit unit in _selectedUnits)
            {
                unit.MoveTo(points[index]);
                _points.Add(points[index]);
                index++;
            }
        }
    }

    private static Vector3[] GetUnitTrainingPoints(Vector3 mouse_point, int size)
    {
        int LINE_X = Mathf.Min(4, size);
        float UNIT_SIZE = 3f;
        float W = UNIT_SIZE * LINE_X;

        float x = mouse_point.x - (W / 2f);
        float z = mouse_point.z;

        Vector3[] points = new Vector3[size];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = mouse_point;
            points[i].x = x;
            points[i].z = z;
            x += UNIT_SIZE;

            if (LINE_X == (i % LINE_X) + 1)
            {
                x = mouse_point.x - (W / 2f);
                z += UNIT_SIZE;
            }
        }

        return points;
    }

    public static bool IsPointerOverGameObject()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return true;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
        }
        return false;
    }
}