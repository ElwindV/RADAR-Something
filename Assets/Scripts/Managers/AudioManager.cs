using System;
using Audio;
using UnityEngine;
using Type = Audio.Type;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables
        
        private AudioSource _efxSource;
        private AudioSource _musicSource;

        [Header("Channels")] 
        [SerializeField]
        private AudioCueEventChannelSO sfxEventChannel;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            sfxEventChannel.onAudioCueRequested += PlayAudioCue;

            _efxSource = gameObject.AddComponent<AudioSource>();
            _musicSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion

        private void PlayAudioCue(AudioCueSO audioCueSO)
        {
            var source = audioCueSO.type switch
            {
                Type.FX => _efxSource,
                Type.Music => _musicSource,
                _ => throw new ArgumentException("Invalid Audio Type"),
            };
            
            source.volume = audioCueSO.volume;
            source.clip = audioCueSO.clip;
            source.Play();
        }
    }
}