using System.Collections.Generic;
using _Scripts.Audio._Scripts.Audio;
using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace _Scripts.Audio
{
    public class AudioManager
    {
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SfxVolume";
        private const string POOLED_AUDIO_SOURCE_NAME = "PooledAudioSource";
        private const float MUTE_DB_LEVEL = -80f; // Mute level in decibels
        private const float MIN_VOLUME_THRESHOLD = 0.0001f; // Minimum threshold to consider sound on

        private readonly AudioMixer _audioMixer;
        private readonly List<AudioSource> _audioSources = new List<AudioSource>();
        private readonly Queue<AudioSource> _audioSourcePool = new Queue<AudioSource>();
        private readonly AudioSource _musicSource;
        private AudioClipCollection _audioClipCollection;
        private float _previousMusicVolume = 1f;
        private float _previousSfxVolume = 1f;

        public AudioManager(AudioMixer audioMixer, AudioSource musicSource)
        {
            _audioMixer = audioMixer;
            _musicSource = musicSource;
            LoadClipCollection();
            InitializePool();
            LoadSettings();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
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
            if (_audioMixer.GetFloat(MUSIC_VOLUME_KEY, out var volume))
            {
                return Mathf.Approximately(volume, MUTE_DB_LEVEL) ? 0f : Mathf.Pow(10, volume / 20f);
            }
            
            return 1f;
        }

        public void SetMusicVolume(float volume)
        {
            Debug.Log($"Setting music volume: {volume}");
            
            if (volume > MIN_VOLUME_THRESHOLD)
            {
                _previousMusicVolume = volume;
                _audioMixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(volume) * 20f);
            }
            else
            {
                _audioMixer.SetFloat(MUSIC_VOLUME_KEY, MUTE_DB_LEVEL);
            }
            
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }

        public float GetSfxVolume()
        {
            if (_audioMixer.GetFloat(SFX_VOLUME_KEY, out var volume))
            {
                return Mathf.Approximately(volume, MUTE_DB_LEVEL) ? 0f : Mathf.Pow(10, volume / 20f);
            }
            
            return 1f;
        }

        public void SetSfxVolume(float volume)
        {
            Debug.Log($"Setting SFX volume: {volume}");
            
            if (volume > MIN_VOLUME_THRESHOLD)
            {
                _previousSfxVolume = volume;
                _audioMixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(volume) * 20f);
            }
            else
            {
                _audioMixer.SetFloat(SFX_VOLUME_KEY, MUTE_DB_LEVEL);
            }
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        }

        public void ToggleMusic()
        {
            if (IsMusicOn())
            {
                Debug.Log($"Toggling music off");
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, 0f);
                SetMusicVolume(0f);
            }
            else
            {
                Debug.Log($"Toggling music on");
                SetMusicVolume(_previousMusicVolume > MIN_VOLUME_THRESHOLD ? _previousMusicVolume : 1f);
            }
        }

        public void ToggleSfx()
        {
            if (IsSfxOn())
            {
                Debug.Log($"Toggling SFX off");
                PlayerPrefs.SetFloat(SFX_VOLUME_KEY, 0f);
                SetSfxVolume(0f);
            }
            else
            {
                Debug.Log($"Toggling SFX on");
                SetSfxVolume(_previousSfxVolume > MIN_VOLUME_THRESHOLD ? _previousSfxVolume : 1f);
            }
        }

        public bool IsMusicOn()
        {
            return GetMusicVolume() > MIN_VOLUME_THRESHOLD;
        }

        public bool IsSfxOn()
        {
            return GetSfxVolume() > MIN_VOLUME_THRESHOLD;
        }

        public void LoadSettings()
        {
            Debug.Log("Loading audio settings");

            float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

            _previousMusicVolume = (musicVolume > MIN_VOLUME_THRESHOLD ? musicVolume : 1f);
            _previousSfxVolume = (sfxVolume > MIN_VOLUME_THRESHOLD ? sfxVolume : 1f);

            SetMusicVolume(musicVolume == 0 ? MIN_VOLUME_THRESHOLD : musicVolume);
            SetSfxVolume(sfxVolume == 0 ? MIN_VOLUME_THRESHOLD : sfxVolume);
        }

        public static void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        public void PlaySfx(string clipName, Vector3 position = default, float volume = 1.0f)
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
            _ = UniTask.Delay((int)(clipLength * 1000)).ContinueWith(() => ReleaseAudioSource(source));
        }
        
        private void PlayMainMusic(string clipName)
        {
            var clip = _audioClipCollection.GetClip(clipName);

            if (clip == null)
            {
                Debug.LogError($"Clip with name {clipName} not found in the collection.");
                return;
            }
            
            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                return;
            }
            
            _musicSource.clip = clip;
            _musicSource.Play();
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene loaded: {scene.name} with index: {scene.buildIndex}");
            
            const int uiSceneIndex = 2;
            const int gameplaySceneIndex = 3;
            
            switch (scene.buildIndex)
            {
                case >= gameplaySceneIndex:
                    PlayMainMusic(AudioConstants.GAMEPLAY_MUSIC);
                    break;
                case < uiSceneIndex:
                    PlayMainMusic(AudioConstants.TITLE_MUSIC);
                    break;
            }
        }
    }
}
