using System;
using Bardent.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Bardent.UI
{
    public class WeaponSwapChoiceUI : MonoBehaviour
    {
        public event Action<int> IndexSelected;

        [SerializeField] private WeaponInfoUI weaponInfoUI;
        [SerializeField] private CombatInputs input;

        public WeaponSwapChoice weaponSwapChoice;

        public void TakeRelevantChoice(WeaponSwapChoice[] choices)
        {
            var inputIndex = (int)input;

            if (choices.Length <= inputIndex)
            {
                return;
            }

            SetChoice(choices[inputIndex]);
        }

        private void SetChoice(WeaponSwapChoice choice)
        {
            weaponSwapChoice = choice;

            weaponInfoUI.PopulateUI(choice.WeaponData);
        }
    }
}