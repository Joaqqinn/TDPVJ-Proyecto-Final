using Bardent.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bardent.UI
{
    public class UISwapWeapon : MonoBehaviour
    {
        private WeaponInfoUI newWeaponInfo;
        [SerializeField] private Button button;

        private void HandleChoiceRequested(WeaponSwapChoiceRequest choiceRequest)
        {

        }
        private void HandleClick()
        {
            Debug.Log("CLICK");
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
