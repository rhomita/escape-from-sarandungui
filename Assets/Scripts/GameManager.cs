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
    
    private Vector3 _endCameraPosition = new Vector3(0, 30, -25); 

    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
    public Rocket Rocket => _rocket;
    public bool Finished { get; private set; }

    void Start()
    {
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
    }

    public void Finish(bool win)
    {
        Finished = true;
        _camera.GetComponent<CameraMovementController>().enabled = false;
        _camera.GetComponent<CameraController>().enabled = false;
        _camera.transform.parent = _rocket.transform;
        _camera.transform.localPosition = _endCameraPosition;
        _canvas.gameObject.SetActive(false);

        if (win)
        {
            
        }
        else
        {
            
        }
    }
}