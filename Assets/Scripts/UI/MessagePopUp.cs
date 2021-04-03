using TMPro;
using UnityEngine;

namespace UI
{
    public class MessagePopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        void OnEnable()
        {
            _text.text = "";
        }
        
        public void SetText(string text)
        {
            _text.text = text;
        }

        public void Hide()
        {
            SimplePool.Despawn(this.gameObject);
        }
    }
}