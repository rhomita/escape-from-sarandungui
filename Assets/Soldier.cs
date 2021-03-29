using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _shootTarget;
    
    void Start()
    {
        _navMeshAgent = transform.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        bool isRunning = _navMeshAgent.velocity.magnitude > 0.1f;
        bool isShooting = false;
        
        _animator.SetBool("isRunning", isRunning);
    }

    public void MoveTo(Vector3 position)
    {
        _navMeshAgent.Move(position);
    }

    public void SetShootTarget(Vector3 position)
    {
    }
    
}
