using System;
using System.Collections;
using UI;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class Rocket : Attackable
{
    [SerializeField] private Transform parts;
    [SerializeField] private ProgressBar _progressBar;
    
    [Header("End")]
    [SerializeField] private GameObject _fireParticles;
    [SerializeField] private GameObject _engineers;
    
    [Header("SFX")]
    [SerializeField] private SoundEffect _launchSound;
    [SerializeField] private SoundEffect _impactSound;

    private float _progress = 0f;
    private float _minRandomProgress = 0.1f;
    private float _maxRandomProgress = 0.5f;
    private float _damageProgressLossFactor = 100f;
    private float _maxProgress = 100f;

    private float _progressPerPart;
    private float _timeToLose = 10f;
    private float _currentCountdown;
    
    void Start()
    {
        InvokeRepeating("MakeProgress", 1, 1);
        _currentCountdown = _timeToLose;
        _progressPerPart = Mathf.Floor(_maxProgress / (parts.childCount - 1));
        UpdateProgress();
    }

    void Update()
    {
        if (!GameManager.Instance.IsActive) return;
        if (_progress >= 100)
        {
            GameManager.Instance.Finish(true);
            StartCoroutine(Launch());
            return;
        }

        if (_progress < 0)
        {
            _currentCountdown -= Time.deltaTime;
        }
        else
        {
            _currentCountdown = _timeToLose;
        }

        if (_currentCountdown <= 0f)
        {
            GameManager.Instance.Finish(false);
            StartCoroutine(Explode());
            return;
        }
    }

    void MakeProgress()
    {
        _progress += Random.Range(_minRandomProgress, _maxRandomProgress);
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        int partsToEnable = (int) Mathf.Floor(_progress / _progressPerPart);

        for (int i = 1; i < parts.childCount; i++)
        {
            parts.GetChild(i).gameObject.SetActive(i <= partsToEnable);
        }

        if (_progress < 0)
        {
            _progressBar.SetProgress(0);
            return;
        }

        _progressBar.SetProgress(_progress);
    }

    public override bool IsDead()
    {
        return false;
    }

    public override void TakeDamage(int damage, Vector3 damageForce, Attackable _attacker = null)
    {
        _progress -= damage / _damageProgressLossFactor;
        UpdateProgress();
    }

    private IEnumerator Launch()
    {
        enabled = false;
        _launchSound.Play();
        _progressBar.gameObject.SetActive(false);
        _fireParticles.SetActive(true);
        _engineers.SetActive(false);
        float seconds = 50f;
        
        yield return new WaitForSeconds(1f);
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + Vector3.up * 1000;

        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / seconds));
            yield return null;
        }

        yield return null;
    }

    private IEnumerator Explode()
    {
        enabled = false;
        _progressBar.gameObject.SetActive(false);
        _engineers.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up);
        
        yield return new WaitForSeconds(1f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up * 1.5f);
        
        yield return new WaitForSeconds(0.5f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up * 1.3f);
        
        yield return new WaitForSeconds(0.3f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up * 1.3f);
        
        yield return new WaitForSeconds(0.5f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up * 1.7f);
        
        yield return new WaitForSeconds(1.5f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up * 1.5f);
        ParticlesManager.Instance.Spawn("explosion", transform.position + Vector3.up * 3f);
        
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Projectile projectile))
        {
            _impactSound.Play();
        }
    }
}