using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _speed = 4f;
    private float _timeToDie = 10f;
    private float _livingTime;
    public float Damage => 10;
    
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

    private void OnCollisionEnter(Collision other)
    {
        // TODO
        Debug.Log(">" + other.gameObject.name);
    }

    public void OnUnitHit()
    {
        SimplePool.Despawn(gameObject);
    }
}
