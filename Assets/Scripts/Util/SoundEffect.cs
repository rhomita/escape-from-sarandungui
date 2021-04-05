using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Util
{
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private List<AudioClip> _clips;
        [SerializeField]
        [Range(0f, 1f)]
        private float volume;
        [SerializeField] [Range(0f, 1f)] private float spatialBlend = 1f;

        private AudioSource _audioSource;
        
        void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = volume;
            _audioSource.loop = false;
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = spatialBlend;
            _audioSource.outputAudioMixerGroup = _audioMixerGroup;
        }
        
        public void Play()
        {
            if (_audioSource.isPlaying) return;
            int random = Random.Range(0, _clips.Count);
            AudioClip clip = _clips[random];
            _audioSource.Stop();
            _audioSource.PlayOneShot(clip);
        }
    }
}