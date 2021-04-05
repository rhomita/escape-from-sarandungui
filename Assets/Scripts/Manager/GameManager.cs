using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Rocket _rocket;
    [SerializeField] private Texture2D _cursor;

    [Header("Pause")]
    [SerializeField] private GameObject _pauseCanvas;
    
    [Header("Finish")] 
    [SerializeField] private GameObject _finishedCanvas;
    [SerializeField] private TextMeshProUGUI _finishedMessage;

    private string _winMessage = "You have successfully escaped!";
    private string _loseMessage = "Your rocket has been destroyed.";

    private Vector3 _endCameraPosition = new Vector3(0, 30, -25);

    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
    public Rocket Rocket => _rocket;
    public bool IsActive => !_isPaused && !_finished;

    private bool _isPaused = false;
    private bool _finished = false;

    void Start()
    {
        _finishedCanvas.SetActive(false);
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
        TogglePause(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
            TogglePause(_isPaused);
        }
    }

    public void TogglePause(bool enable)
    {
        Time.timeScale = enable ? 0 : 1;
        _pauseCanvas.SetActive(enable);
        _isPaused = enable;
    }

    public void Finish(bool win)
    {
        _finished = true;
        _camera.GetComponent<CameraMovementController>().enabled = false;
        _camera.GetComponent<CameraController>().enabled = false;
        _camera.transform.parent = _rocket.transform;
        _camera.transform.localPosition = _endCameraPosition;
        _canvas.gameObject.SetActive(false);

        _finishedCanvas.SetActive(true);
        _finishedMessage.text = win ? _winMessage : _loseMessage;
    }
}