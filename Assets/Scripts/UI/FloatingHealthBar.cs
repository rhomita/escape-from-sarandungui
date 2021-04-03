using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FloatingHealthBar : FloatingUI
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _sliderImage;
        [SerializeField] private Vector3 _offset;

        private int _maxHealth;

        void Update()
        {
            transform.rotation = Quaternion.identity;
            transform.position = transform.root.position + _offset;
        }

        public void SetMaxHealth(float health)
        {
            _maxHealth = (int) health;
            _healthSlider.maxValue = _maxHealth;
        }

        public void SetHealth(float health)
        {
            int value = (int) health;
            _healthSlider.value = value;

            if (value == _maxHealth || value <= 0)
            {
                _healthSlider.gameObject.SetActive(false);
            }
            else
            {
                _healthSlider.gameObject.SetActive(true);
            }
            
            _sliderImage.color = Color.Lerp(Color.red, Color.green, _healthSlider.value / _maxHealth);
        }
    }
}