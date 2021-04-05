using UnityEngine;

public class ParticlesAutoDestroy : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = transform.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_particleSystem.IsAlive()) return;
        SimplePool.Despawn(this.gameObject);
    }
}
