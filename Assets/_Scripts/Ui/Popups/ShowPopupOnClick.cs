using _Scripts.Infra;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui.Popups
{
    [RequireComponent(typeof(Button))]
    public class ShowPopupOnClick : MonoBehaviour
    {
        [SerializeField] private EPopup popupType;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();

            if (_button != null)
            {
                _button.onClick.AddListener(OnButtonClick);
            }
            else
            {
                Debug.LogError("Button component is missing.");
            }
        }

        private async void OnButtonClick()
        {
            var popupManager = ServiceLocator.GetService<PopupManager>();

            if (popupManager != null)
            {
                await popupManager.ShowPopupAsync(popupType);
            }
            else
            {
                Debug.LogError("PopupManager is not registered in the ServiceLocator.");
            }
        }
    }
}