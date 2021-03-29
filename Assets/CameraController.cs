using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    
public class CameraController : MonoBehaviour
{
    public Soldier test;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast (ray, out RaycastHit _hit, 200))
            {
                Vector3 endPosition = _hit.point;
                bool blocked = NavMesh.Raycast(transform.position, endPosition, out NavMeshHit hit, NavMesh.AllAreas);
                if (blocked) return; // Cannot move there
                test.MoveTo(endPosition);
            }    
        }

        if (Input.GetMouseButtonDown(1))
        {
            
        }
    }
}
