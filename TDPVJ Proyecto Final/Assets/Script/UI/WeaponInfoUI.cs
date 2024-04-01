using Bardent.Weapons;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bardent.UI
{
    public class WeaponInfoUI : MonoBehaviour
    {
        public event Action<int> IndexSelected;

        [Header("Dependencies")] [SerializeField]
        private Image weaponIcon;

        [SerializeField] private TMP_Text weaponName;
        [SerializeField] private TMP_Text weaponDescription;
        [SerializeField] private Button button;
        [SerializeField] public int Index;

        private WeaponDataSO weaponData;

        public void PopulateUI(WeaponDataSO data)
        {
            if(data is null)
                return;

            weaponData = data;

            weaponIcon.sprite = weaponData.Icon;
            weaponName.SetText(weaponData.Name);
            weaponDescription.SetText(weaponData.Description);
        }

        private void HandleClick()
        {
            IndexSelected?.Invoke(Index);
        }

        private void OnEnable()
        {
            button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(HandleClick);
        }
    }
}