using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;
    
    public void SetEnabled(bool enabled)
    {
        if (_rigidbodies == null)
        {
            _rigidbodies = transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
            _colliders = transform.GetChild(0).GetComponentsInChildren<Collider>();
        }
        
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        foreach (Collider collider in _colliders)
        {
            collider.enabled = enabled;
        }

        animator.enabled = !enabled;
    }

    public void AddImpulse(Vector3 force)
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}