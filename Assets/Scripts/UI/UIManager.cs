using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform _messagesPopupContainer;
        [SerializeField] private GameObject _messagePopupPrefab;

        public static UIManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowMessagePopup(string text)
        {
            MessagePopUp messagePopUp = SimplePool.Spawn(_messagePopupPrefab, _messagesPopupContainer.position, Quaternion.identity)
                .GetComponent<MessagePopUp>();
            messagePopUp.transform.parent = _messagesPopupContainer;
            messagePopUp.transform.localPosition = Vector3.zero;
            messagePopUp.SetText(text);
        }
    }
}