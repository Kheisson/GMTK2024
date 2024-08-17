using System.Collections.Generic;
using _Scripts.Audio._Scripts.Audio;
using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Audio
{
    public class AudioManager
    {
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SfxVolume";
        private const string POOLED_AUDIO_SOURCE_NAME = "PooledAudioSource";

        private readonly AudioMixer _audioMixer;
        private readonly List<AudioSource> _audioSources = new List<AudioSource>();
        private readonly Queue<AudioSource> _audioSourcePool = new Queue<AudioSource>();
        private AudioClipCollection _audioClipCollection;
        
        public AudioManager(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
            LoadClipCollection();
            InitializePool();
        }

        private void InitializePool(int poolSize = 10)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var newSource = CreateAnAudioSource();
                _audioSourcePool.Enqueue(newSource);
            }
        }
        
        private void LoadClipCollection()
        {
            _audioClipCollection = Resources.Load<AudioClipCollection>(nameof(AudioClipCollection));
        }

        private static AudioSource CreateAnAudioSource()
        {
            AudioSource newSource = new GameObject(POOLED_AUDIO_SOURCE_NAME).AddComponent<AudioSource>();
            newSource.gameObject.transform.parent = GameContainer.Instance.transform;
            newSource.playOnAwake = false;
            return newSource;
        }

        private AudioSource GetAudioSource()
        {
            if (_audioSourcePool.Count > 0)
            {
                return _audioSourcePool.Dequeue();
            }
            
            return CreateAnAudioSource();
        }

        private void ReleaseAudioSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            _audioSourcePool.Enqueue(source);
        }

        public float GetMusicVolume()
        {
            _audioMixer.GetFloat(MUSIC_VOLUME_KEY, out var volume);
            return Mathf.Pow(10, volume / 20);
        }

        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        
        public float GetSfxVolume()
        {
            _audioMixer.GetFloat(SFX_VOLUME_KEY, out var volume);
            return Mathf.Pow(10, volume / 20);
        }

        public void SetSfxVolume(float volume)
        {
            _audioMixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        }

        public void ToggleMusic(bool isOn)
        {
            SetMusicVolume(isOn ? PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f) : 0f);
        }

        public void ToggleSfx(bool isOn)
        {
            SetSfxVolume(isOn ? PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f) : 0f);
        }

        public bool IsMusicOn()
        {
            _audioMixer.GetFloat(MUSIC_VOLUME_KEY, out var volume);
            return volume > Mathf.Log10(0.1f) * 20;
        }

        public bool IsSfxOn()
        {
            _audioMixer.GetFloat(SFX_VOLUME_KEY, out var volume);
            return volume > Mathf.Log10(0.1f) * 20;
        }

        public void LoadSettings()
        {
            var musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            var sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        
            SetMusicVolume(musicVolume);
            SetSfxVolume(sfxVolume);
        }

        public void PlaySfx(string clipName, Vector3 position = default(Vector3), float volume = 1.0f)
        {
            var clip = _audioClipCollection.GetClip(clipName);
            
            if (clip == null)
            {
                Debug.LogError($"Clip with name {clipName} not found in the collection.");
                return;
            }
            
            var source = GetAudioSource();
            source.transform.position = position;
            source.clip = clip;
            source.volume = volume * GetSfxVolume();
            source.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Sfx")[0];
            source.Play();
            _audioSources.Add(source);

            var clipLength = clip.length;
            _ = UniTask.Delay((int) (clipLength * 1000)).ContinueWith(() => ReleaseAudioSource(source));
        }
    }
}
