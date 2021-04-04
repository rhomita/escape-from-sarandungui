using TMPro;
using UnityEngine;

namespace UI
{
    public class MoneyPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        private Camera _camera;
        private Vector3 worldPosition;

        void Awake()
        {
            _camera = GameManager.Instance.Camera;
            transform.parent = GameManager.Instance.Canvas.transform;
        }

        public void Show(Vector3 position, float money)
        {
            worldPosition = position;
            moneyText.text = $"+ $ {(int) money}";
        }

        void Update()
        {
            transform.position = _camera.WorldToScreenPoint(worldPosition);
        }

        void Hide()
        {
            SimplePool.Despawn(this.gameObject);
        }
    }
}