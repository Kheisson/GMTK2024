using System;
using _Scripts.Audio;
using _Scripts.Infra;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui.Popups
{
    public class SettingsPopup : PopupView
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Button sfxButton;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Sprite musicOnIcon;
        [SerializeField] private Sprite musicOffIcon;
        [SerializeField] private Sprite sfxOnIcon;
        [SerializeField] private Sprite sfxOffIcon;

        private AudioManager _audioManager;
        private Image _musicButtonImage;
        private Image _sfxButtonImage;

        private void Start()
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
            musicButton.onClick.AddListener(OnMusicButtonClick);
            sfxButton.onClick.AddListener(OnSfxButtonClick);
            musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChange);

            _audioManager = ServiceLocator.GetService<AudioManager>();
            _musicButtonImage = musicButton.GetComponent<Image>();
            _sfxButtonImage = sfxButton.GetComponent<Image>();

            LoadSettings();
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void LoadSettings()
        {
            if (_audioManager == null) return;
            
            _audioManager.LoadSettings();

            var musicVolume = _audioManager.GetMusicVolume();
            var sfxVolume = _audioManager.GetSfxVolume();

            musicSlider.value = musicVolume;
            sfxSlider.value = sfxVolume;

            UpdateMusicButtonIcon(_audioManager.IsMusicOn());
            UpdateSfxButtonIcon(_audioManager.IsSfxOn());
        }

        private async void OnCloseButtonClick()
        {
            Time.timeScale = 1; 
            var popupManager = ServiceLocator.GetService<PopupManager>();
            await popupManager.ClosePopupAsync();
        }

        private void OnMusicButtonClick()
        {
            if (_audioManager != null)
            {
                bool isOn = !_audioManager.IsMusicOn();
                _audioManager.ToggleMusic(isOn);
                UpdateMusicButtonIcon(isOn);
            }
        }

        private void OnSfxButtonClick()
        {
            if (_audioManager != null)
            {
                bool isOn = !_audioManager.IsSfxOn();
                _audioManager.ToggleSfx(isOn);
                UpdateSfxButtonIcon(isOn);
            }
        }

        private void OnMusicSliderChange(float value)
        {
            if (_audioManager != null)
            {
                _audioManager.SetMusicVolume(value);
                UpdateMusicButtonIcon(value > 0);
            }
        }

        private void OnSfxSliderChange(float value)
        {
            if (_audioManager != null)
            {
                _audioManager.SetSfxVolume(value);
                UpdateSfxButtonIcon(value > 0);
            }
        }

        private void UpdateMusicButtonIcon(bool isOn)
        {
            if (_musicButtonImage != null)
            {
                _musicButtonImage.sprite = isOn ? musicOnIcon : musicOffIcon;
            }
        }

        private void UpdateSfxButtonIcon(bool isOn)
        {
            if (_sfxButtonImage != null)
            {
                _sfxButtonImage.sprite = isOn ? sfxOnIcon : sfxOffIcon;
            }
        }
    }
}
