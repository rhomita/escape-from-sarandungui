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

    public Camera Camera => _camera;
    public Canvas Canvas => _canvas;
}