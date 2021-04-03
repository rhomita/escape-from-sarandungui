using UnityEngine;

namespace UI
{
    public class FloatingUI : MonoBehaviour
    {
        [SerializeField] private bool _lookAtCamera = false;
        private Transform _camera;
    
        void Start()
        {
            _camera = GameManager.Instance.Camera.transform;
        }

        void LateUpdate()
        {
            if (!_lookAtCamera) return;
            transform.LookAt(_camera);
        }
    }
}