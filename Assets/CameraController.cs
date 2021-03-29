using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    
public class CameraController : MonoBehaviour
{
    // listado de unidades a mover
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast (ray, out RaycastHit _hit, 200))
            {
                Vector3 endPosition = _hit.point;
                bool blocked = NavMesh.Raycast(transform.position, endPosition, out NavMeshHit hit, NavMesh.AllAreas);
                Debug.DrawLine(transform.position, endPosition, blocked ? Color.red : Color.green);

                if (blocked)
                    Debug.DrawRay(hit.position, Vector3.up, Color.red);
            }    
        }

        if (Input.GetMouseButtonDown(1))
        {
            
        }
    }
}
