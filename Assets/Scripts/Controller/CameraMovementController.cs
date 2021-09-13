using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private float _speed;
    [SerializeField] private float _androidSpeed;
    [SerializeField] private float _borderSize;
    [SerializeField] private Vector2 _borderLimit;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;

    private Vector3 _lastTouchPosition;

    void Update()
    {
        Vector3 position = transform.position;

        if (GameManager.Instance.IsAndroid)
        {
            if (Input.touchCount >= 2)
            {
                if (Input.touchCount > 2)
                {
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    _lastTouchPosition = Input.GetTouch(0).position;
                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 currentPosition = Input.GetTouch(0).position;
                    Vector3 direction = _lastTouchPosition - currentPosition;
                    _lastTouchPosition = currentPosition;
                    position.z += direction.y * _androidSpeed * Time.deltaTime;
                    position.x += direction.x * _androidSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - _borderSize)
            {
                position.z += _speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= _borderSize)
            {
                position.z -= _speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - _borderSize)
            {
                position.x += _speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= _borderSize)
            {
                position.x -= _speed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            position.y += -scroll * _scrollSpeed * 100f * Time.deltaTime;
        }


        position.y = Mathf.Clamp(position.y, _minHeight, _maxHeight);
        // _minimapCamera.orthographicSize = position.y; // For now I comment it out. It might look better without zooming it.

        position.x = Mathf.Clamp(position.x, -_borderLimit.x, _borderLimit.x);
        position.z = Mathf.Clamp(position.z, -_borderLimit.y, _borderLimit.y);

        transform.position = position;
    }
}