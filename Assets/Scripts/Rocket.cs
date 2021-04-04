using UI;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Transform parts;
    [SerializeField] private ProgressBar _progressBar;

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
        if (_progress >= 100)
        {
            Debug.Log("WIN");
            enabled = false;
            _progressBar.gameObject.SetActive(false);
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
            Debug.Log("LOSE"); // TODO:
        }
    }

    void MakeProgress()
    {
        _progress += Random.Range(_minRandomProgress, _maxRandomProgress);
        UpdateProgress();
    }

    void TakeDamage(float _damage)
    {
        _progress -= _damage / _damageProgressLossFactor;
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
}