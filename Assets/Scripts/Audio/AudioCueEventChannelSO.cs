using UnityEngine;
using UnityEngine.Events;

namespace Audio
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Channels/Create Audio Channel", order = 1)]
    public class AudioCueEventChannelSO : ScriptableObject
    {
        public UnityAction<AudioCueSO> onAudioCueRequested;

        public void RaiseEvent(AudioCueSO audioCue)
        {
            if (onAudioCueRequested != null)
            {
                onAudioCueRequested.Invoke(audioCue);
            }
            else
            {
                Debug.LogWarning("Tried to play a sound, but there is no listener.");
            }
        }
    }
}