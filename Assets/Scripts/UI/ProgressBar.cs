using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : FloatingUI
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _progressText;

        protected override void Start()
        {
            base.Start();
            _slider.maxValue = 100f;
        }

        public void SetProgress(float progress)
        {
            _slider.value = progress;
            _progressText.text = $"{(int) progress} / 100";
        }
    }
}