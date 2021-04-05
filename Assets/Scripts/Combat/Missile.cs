using System.Collections.Generic;
using UnityEngine;
using Util;

public class Missile : Projectile
{
    [Header("SFX")] 
    [SerializeField] private SoundEffect _launchSoundEffect;

    private float _minSpeed = 2.2f;
    private float _maxSpeed = 4f;
    private float _damageForce = 15f;
    private float _damageUpForce = 5f;
    private float _explosionRadius = 2f;


    protected override void OnInit()
    {
        _minDamage = 20;
        _maxDamage = 40;
        _launchSoundEffect.Play();
    }

    protected override void OnUpdate()
    {
        float force = Mathf.Lerp(_minSpeed, _maxSpeed, _livingTime);
        force = Mathf.Clamp(force, _minSpeed, _maxSpeed);
        _rigidbody.AddForce(transform.forward * force);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!_initialized) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        int damage = GetDamage();
        bool foundOneValidAttackableTarget = false;
        foreach (Collider _collider in colliders)
        {
            if (_collider.TryGetComponent(out Attackable attackable))
            {
                if (attackable.TryGetComponent(out Unit unit))
                {
                    if (unit.Team.Number == _owner.Team.Number) continue;
                }

                foundOneValidAttackableTarget = true;
                Vector3 direction = (collider.transform.position - transform.position).normalized +
                                    (Vector3.up * _damageUpForce);
                attackable.TakeDamage(damage, direction * _damageForce, _owner);
            }
        }

        if (foundOneValidAttackableTarget)
        {
            _launchSoundEffect.Stop();
            ParticlesManager.Instance.Spawn("explosion", collider.transform.position);
            SimplePool.Despawn(gameObject);
        }
    }
}