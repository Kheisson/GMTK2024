using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.Audio
{
    public class AudioManager
    {
        private readonly AudioMixer _audioMixer;
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SfxVolume";

        public AudioManager(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
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

        public void SetSfxVolume(float volume)
        {
            _audioMixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        }
        
        public float GetSfxVolume()
        {
            _audioMixer.GetFloat(SFX_VOLUME_KEY, out var volume);
            return Mathf.Pow(10, volume / 20);
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
    }

}