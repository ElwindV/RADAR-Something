using System;
using Audio;
using UnityEngine;
using Type = Audio.Type;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] [Range(1, 16)] private ushort poolSize = 8;

        private AudioSource[] audioSources;
        private ushort audioSourcePointer = 0;

        private AudioSource _musicSource;

        [Header("Channels")] [SerializeField] private AudioCueEventChannelSO sfxEventChannel;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            sfxEventChannel.onAudioCueRequested += PlayAudioCue;

            audioSources = new AudioSource[poolSize];
            for (var i = 0; i < poolSize; i++)
            {
                var source = new GameObject("SFX Pool Object");
                source.transform.SetParent(transform);
                audioSources[i] = source.AddComponent<AudioSource>();
            }
            
            _musicSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion

        private AudioSource GetFromPool()
        {
            audioSourcePointer += 1;
            audioSourcePointer %= poolSize;

            return audioSources[audioSourcePointer];
        }

        private void PlayAudioCue(AudioCueSO audioCueSO)
        {
            var source = audioCueSO.type switch
            {
                Type.FX => GetFromPool(),
                Type.Music => _musicSource,
                _ => throw new ArgumentException("Invalid Audio Type"),
            };
            
            source.volume = audioCueSO.volume;
            source.clip = audioCueSO.clip;
            source.Play();
        }
    }
}