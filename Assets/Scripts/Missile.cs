using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _minSpeed = 1.6f;
    private float _maxSpeed = 15f;
    private float _timeToDie = 10f;
    private float _livingTime;
    private int _minDamage = 20;
    private int _maxDamage = 40;
    private float _damageForce = 15f;
    private float _damageUpForce = 5f;
    private float _explosionRadius = 2f;

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider _collider in colliders)
        {
            if (_collider.TryGetComponent(out Unit unit))
            {
                int damage = Random.Range(_minDamage, _maxDamage);
                Vector3 direction = (collider.transform.position - transform.position).normalized + (Vector3.up * _damageUpForce);
                unit.TakeDamage(damage, direction * _damageForce);
                SimplePool.Despawn(gameObject);    
            }
        }
    }
}