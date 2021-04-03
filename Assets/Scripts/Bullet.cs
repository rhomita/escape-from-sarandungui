using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _speed = 6f;
    private float _timeToDie = 10f;
    private float _livingTime;
    private int _minDamage = 5;
    private int _maxDamage = 15;
    private float _damageForce = 70f;
    
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
    }

    private void OnEnable()
    {
        _rigidbody.velocity = Vector3.zero;
        _livingTime = 0;
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Unit unit))
        {
            int damage = Random.Range(_minDamage, _maxDamage);
            Vector3 direction = collider.transform.position - transform.position;
            unit.TakeDamage(damage, direction.normalized * _damageForce);
            SimplePool.Despawn(gameObject);
        }
    }
}
