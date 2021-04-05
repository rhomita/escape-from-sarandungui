using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [SerializeField] private GameObject _explosionParticles;

    private Dictionary<string, GameObject> _particles;
    
    public static ParticlesManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _particles = new Dictionary<string, GameObject>();
        _particles.Add("explosion", _explosionParticles);
    }

    public void Spawn(string particlesName, Vector3 position)
    {
        if (!_particles.ContainsKey(particlesName)) return;
        SimplePool.Spawn(_particles[particlesName], position, Quaternion.identity);
    }
}
