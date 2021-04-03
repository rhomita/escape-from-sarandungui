using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _minSpeed = 1.6f;
    private float _maxSpeed = 15f;
    private float _timeToDie = 10f;
    private float _livingTime;
    private int _damage = 10;

    private float _timeToIncreaseSpeed = 1.5f;
    
    void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        _livingTime += Time.deltaTime;
        if (_livingTime > _timeToDie)
        {
            SimplePool.Despawn(gameObject);
        }

        float force = Mathf.Lerp(_minSpeed, _maxSpeed, _livingTime);
        _rigidbody.AddForce(transform.forward * force);
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        _livingTime = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Unit unit))
        {
            unit.TakeDamage(_damage);
            SimplePool.Despawn(gameObject);
        }
    }
}