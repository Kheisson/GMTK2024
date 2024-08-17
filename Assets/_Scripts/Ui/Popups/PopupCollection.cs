using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Ui.Popups
{
    [CreateAssetMenu(fileName = "PopupCollection", menuName = "ScriptableObjects/PopupCollection", order = 1)]
    public class PopupCollection : ScriptableObject
    {
        [Serializable]
        public class PopupEntry
        {
            public EPopup id;
            public PopupView popupPrefab;
        }

        public List<PopupEntry> popups;

        private Dictionary<EPopup, PopupView> _popupDictionary;

        private void OnEnable()
        {
            _popupDictionary = new Dictionary<EPopup, PopupView>();

            foreach (var entry in popups.Where(entry => !_popupDictionary.ContainsKey(entry.id)))
            {
                _popupDictionary.Add(entry.id, entry.popupPrefab);
            }
        }

        public PopupView GetPopup(EPopup id)
        {
            if (_popupDictionary.TryGetValue(id, out var popup))
            {
                return popup;
            }

            Debug.LogError($"Popup with ID {id} not found in collection.");
            return null;
        }
    }
}