using _Scripts.Infra;
using _Scripts.Ui.Popups;
using UnityEngine;

namespace _Scripts.Ui
{
    public class BeforeCutscene : MonoBehaviour
    {
        private bool ShowedLayoutInCutscene => PlayerPrefs.GetInt("ShowedLayoutInCutscene", 0) == 1;

        private void Start()
        {
            if (ShowedLayoutInCutscene)
            {
                return;
            }

            PlayerPrefs.SetInt("ShowedLayoutInCutscene", 1);
            _ = ServiceLocator.GetService<PopupManager>().ShowPopupAsync(EPopup.Layout);
        }
    }
}