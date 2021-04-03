using TMPro;
using UnityEngine;

namespace UI
{
    public class DamagePopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;

        private Camera _camera;
        private Vector3 worldPosition;
        private float _damageScaleFactor = 10f;
        private float _randomPositionFactor = .4f;

        void Awake()
        {
            _camera = GameManager.Instance.Camera;
            transform.parent = GameManager.Instance.Canvas.transform;
        }

        public void Show(Vector3 position, float damage)
        {
            worldPosition = position;
            worldPosition += new Vector3(Random.Range(-_randomPositionFactor, _randomPositionFactor),
                Random.Range(-_randomPositionFactor, _randomPositionFactor), 0);
            damageText.text = ((int) damage).ToString();
            damageText.transform.localScale *= (damage / _damageScaleFactor);
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