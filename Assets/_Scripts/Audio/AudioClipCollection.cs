using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Audio
{
    namespace _Scripts.Audio
    {
        [CreateAssetMenu(fileName = "AudioClipCollection", menuName = "Audio/AudioClipCollection", order = 1)]
        public class AudioClipCollection : ScriptableObject
        {
            public List<AudioClip> audioClips;

            public AudioClip GetClip(string clipName)
            {
                return audioClips.Find(clip => clip.name == clipName);
            }
        }
    }

}