using System;
using System.Reflection;
using Bardent.Interaction;
using Bardent.Interaction.Interactables;
using Bardent.UI;
using Bardent.Weapons;
using UnityEngine;

namespace Bardent.CoreSystem
{
    public class WeaponSwap : CoreComponent
    {
        public event Action<WeaponSwapChoiceRequest> OnChoiceRequested;
        public event Action<WeaponDataSO> OnWeaponDiscarded;

        private InteractableDetector interactableDetector;
        private WeaponInventory weaponInventory;

        private WeaponDataSO newWeaponData;

        private WeaponPickup weaponPickup;

        [SerializeField] private WeaponInfoUI[] newWeaponInfo;

        private int index;

        private void HandleTryInteract(IInteractable interactable)
        {
            //if (!(interactable is WeaponPickup pickup))
            //    return;

            //weaponPickup = pickup;

            //newWeaponData = weaponPickup.GetContext();

            if (weaponInventory.TryGetEmptyIndex(out var index))
            {
                weaponInventory.TrySetWeapon(newWeaponData, index, out _);
                interactable.Interact();
                newWeaponData = null;
                return;
            }
            //CAMBIA LA INFORMACION QUE APARECE EN EL RECUADRO
            OnChoiceRequested?.Invoke(new WeaponSwapChoiceRequest(
                HandleWeaponSwapChoice,
                weaponInventory.GetWeaponSwapChoices(),
                weaponInventory.weaponData
            ));
        }

        //SE ENCARGA DE HACER EL SWAP DE ARMAS Y TAMBIEN MANEJA EL ICONO CON EL QUE INTERACTUAMOS
        private void HandleWeaponSwapChoice(WeaponSwapChoice choice)
        {
            //Cambiamos el arma nueva por la vieja
            if (!weaponInventory.TrySetWeapon(weaponInventory.weaponData[index], choice.Index, out var oldData)) 
                return;
            //Ponemos el arma vieja en la posicion de anterior de la nueva
            if (!weaponInventory.TrySetWeapon(oldData, index, out weaponInventory.weaponData[index]))
                return;

            newWeaponData = null;

            //OnWeaponDiscarded?.Invoke(oldData);
                
            if (weaponPickup is null)
                return;

            weaponPickup.Interact();
            
        }
        private void GetIndex(int i)
        {
            index = i;
        }

        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();
            weaponInventory = core.GetCoreComponent<WeaponInventory>();
        }

        private void OnEnable()
        {
            interactableDetector.OnTryInteract += HandleTryInteract;

            foreach (WeaponInfoUI NewweaponInfoUI in newWeaponInfo)
            {
                NewweaponInfoUI.IndexSelected += GetIndex;
            }  
        }


        private void OnDisable()
        {
            interactableDetector.OnTryInteract -= HandleTryInteract;

            foreach (WeaponInfoUI NewweaponInfoUI in newWeaponInfo)
            {
                NewweaponInfoUI.IndexSelected -= GetIndex;
            }
        }
    }
}