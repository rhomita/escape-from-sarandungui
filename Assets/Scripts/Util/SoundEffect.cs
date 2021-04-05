using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Util
{
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        [SerializeField] private List<AudioClip> _clips;
        [SerializeField]
        [Range(0f, 1f)]
        private float _volume;
        [SerializeField] [Range(0f, 1f)] private float _spatialBlend = 1f;
        [SerializeField] private bool _playOnAwake = false;

        private AudioSource _audioSource;
        
        void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = _volume;
            _audioSource.loop = false;
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = _spatialBlend;
            _audioSource.outputAudioMixerGroup = _audioMixerGroup;
        }

        private void OnEnable()
        {
            if (_playOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            if (_audioSource.isPlaying) return;
            int random = Random.Range(0, _clips.Count);
            AudioClip clip = _clips[random];
            _audioSource.Stop();
            _audioSource.PlayOneShot(clip);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}