using System;
using Bardent.CoreSystem;
using Bardent.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Bardent.UI
{
    public class WeaponSwapUI : MonoBehaviour
    {
        [SerializeField] private WeaponSwap weaponSwap;
        [SerializeField] private WeaponInfoUI[] newWeaponInfo;
        [SerializeField] private WeaponSwapChoiceUI weaponSwapChoiceUIs;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Button[] button;

        private CanvasGroup canvasGroup;
        private int index;
        
        private Action<WeaponSwapChoice> choiceSelectedCallback;
        public event Action<WeaponSwapChoice> OnChoiceSelected;

        private void HandleChoiceRequested(WeaponSwapChoiceRequest choiceRequest)
        {
            gameManager.ChangeState(GameManager.GameState.UI);
            
            choiceSelectedCallback = choiceRequest.Callback;

            for (int i = 0; i < newWeaponInfo.Length; i++)
            {
                newWeaponInfo[i].PopulateUI(choiceRequest.NewWeaponData[i + 3]);
            }

            weaponSwapChoiceUIs.TakeRelevantChoice(choiceRequest.Choices);

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
        }
        //SALIMOS DE LA PAUSA EN UI
        private void HandleChoiceSelected(WeaponSwapChoice choice)
        {
            gameManager.ChangeState(GameManager.GameState.Gameplay);
            
            choiceSelectedCallback?.Invoke(choice);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
        }

        private void GetIndex(int i)
        {
            index = i;
        }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
            weaponSwap.OnChoiceRequested += HandleChoiceRequested;

            OnChoiceSelected += HandleChoiceSelected;

            foreach (var NewWeaponInfo in newWeaponInfo)
            {
                NewWeaponInfo.IndexSelected += GetIndex;
            }

            foreach (var Button in button)
            {
                Button.onClick.AddListener(HandleClick);
            }
        }

        private void OnDisable()
        {
            weaponSwap.OnChoiceRequested -= HandleChoiceRequested;

            OnChoiceSelected -= HandleChoiceSelected;

            foreach (var NewWeaponInfo in newWeaponInfo)
            {
                NewWeaponInfo.IndexSelected -= GetIndex;
            }

            foreach (var Button in button)
            {
                Button.onClick.RemoveListener(HandleClick);
            }
        }

        private void HandleClick()
        {
            OnChoiceSelected?.Invoke(weaponSwapChoiceUIs.weaponSwapChoice);
        }
    }
}