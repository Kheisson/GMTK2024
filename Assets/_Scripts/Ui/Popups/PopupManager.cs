using System;
using System.Collections.Generic;
using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Ui.Popups
{
    public class PopupManager
    {
        public event Action OnPopupOpen;
        public event Action OnPopupClose;
        
        private readonly Stack<PopupView> _popups = new Stack<PopupView>();
        private readonly PopupCollection _popupCollection; 
        
        private UiManager _uiManager;
        
        private UiManager UiManager => _uiManager ??= ServiceLocator.GetService<UiManager>();
        
        public PopupManager(PopupCollection collection)
        {
            _popupCollection = collection;
        }

        public async UniTask ShowPopupAsync(EPopup id)
        {
            var popupPrefab = _popupCollection.GetPopup(id);
            
            if (popupPrefab == null || IsPopupOpen(popupPrefab)) return;

            var popupInstance = Object.Instantiate(popupPrefab, UiManager.GetSettingsCanvas().gameObject.transform);
            
            if (_popups.Count > 0)
            {
                _popups.Peek().gameObject.SetActive(false);
            }

            _popups.Push(popupInstance);
            popupInstance.gameObject.SetActive(true);
            OnPopupOpen?.Invoke();
            await popupInstance.ShowAsync();
            Time.timeScale = 0;
        }

        public async UniTask ClosePopupAsync()
        {
            if (_popups.Count == 0) return;

            var currentPopup = _popups.Pop();
            await currentPopup.HideAsync();
            OnPopupClose?.Invoke();
            currentPopup.gameObject.SetActive(false);
            Object.Destroy(currentPopup.gameObject);

            if (_popups.Count > 0)
            {
                _popups.Peek().gameObject.SetActive(true);
            }
        }
        
        private bool IsPopupOpen(PopupView popupPrefab)
        {
            return _popups.Count > 0 && _popups.Peek().GetType() == popupPrefab.GetType();
        }
    }
}