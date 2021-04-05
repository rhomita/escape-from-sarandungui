using System.Collections.Generic;
using UnityEngine;
using Util;

public class Missile : Projectile
{
    [Header("Mesh")] 
    [SerializeField] private MeshRenderer _topMesh;
    [SerializeField] private MeshRenderer _middleMesh;
    [SerializeField] private MeshRenderer _bottomMesh;
    [Header("SFX")] 
    [SerializeField] private SoundEffect _launchSoundEffect;

    private float _minSpeed = 2.2f;
    private float _maxSpeed = 4f;
    private float _damageForce = 60f;
    private float _damageUpForce = 120f;
    private float _explosionRadius = 2f;


    protected override void OnInit()
    {
        _minDamage = 20;
        _maxDamage = 40;
        _launchSoundEffect.Play();
    }
    
    public override void Init(Unit owner)
    {
        base.Init(owner);
        Team team = _owner.Team;
        Material[] materials = _topMesh.materials;
        materials[0] = team.Material;
        _topMesh.materials = materials;
        
        materials = _middleMesh.materials;
        materials[1] = team.Material;
        _middleMesh.materials = materials;
        
        materials = _bottomMesh.materials;
        materials[2] = team.Material;
        _bottomMesh.materials = materials;
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
                Vector3 damageForce = (collider.transform.position - transform.position).normalized * _damageForce +
                                      Vector3.up * _damageUpForce;
                attackable.TakeDamage(damage, damageForce, _owner);
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