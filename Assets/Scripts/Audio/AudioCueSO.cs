using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Audio/Create Audio", fileName = "AudioSO", order = 50)]
    public class AudioCueSO : ScriptableObject
    {
        #region Variables

        public Type type;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;

        #endregion
    }

    public enum Type
    {
        FX,
        Music
    }
}