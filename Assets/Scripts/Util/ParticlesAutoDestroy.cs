using System.Collections;
using UnityEngine;

public class ParticlesAutoDestroy : MonoBehaviour
{
    [SerializeField] private float _extraTimeBeforeDestroy = 0f;
    private ParticleSystem _particleSystem;

    void Start()
    {
        _particleSystem = transform.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_particleSystem.IsAlive()) return;
        StartCoroutine(DestroyParticles(_extraTimeBeforeDestroy));
    }
    
    IEnumerator DestroyParticles(float seconds) {
        yield return new WaitForSeconds(seconds);
        SimplePool.Despawn(this.gameObject);
    }
}
