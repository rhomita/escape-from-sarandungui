using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
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
    }
}