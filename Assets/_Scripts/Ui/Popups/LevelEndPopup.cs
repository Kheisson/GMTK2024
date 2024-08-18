using System;
using _Scripts.Infra;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui.Popups
{
    public class LevelEndPopup : PopupView
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private GameObject playerX;
        [SerializeField] private GameObject playerY;

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }
        
        private async void OnRestartButtonClick()
        {
            var sceneLoader = ServiceLocator.GetService<SceneLoader>();
            await sceneLoader.ReloadCurrentScene();
        }
    }
}