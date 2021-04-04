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

    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
    public Rocket Rocket => _rocket;

    void Start()
    {
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
    }
}