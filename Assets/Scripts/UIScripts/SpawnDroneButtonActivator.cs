using System;
using UnityEngine;

public class SpawnDroneButtonActivator : MonoBehaviour
{
    [SerializeField] SpawnDroneButton _spawnDroneButton;
    [SerializeField] InteractableHandler _interactableHandler;

    public event Action<Base> BaseSelected;

    private void OnEnable()
    {
        _interactableHandler.InteractableSelected += ActivateButton;
        _interactableHandler.InteractableUnselected += DeactivateButton;
    }

    private void OnDisable()
    {
        _interactableHandler.InteractableSelected -= ActivateButton;
        _interactableHandler.InteractableUnselected -= DeactivateButton;
    }

    private void ActivateButton(Interactable interactable)
    {
        if (interactable is Base selectedBase)
        {
            _spawnDroneButton.gameObject.SetActive(true);
            BaseSelected?.Invoke(selectedBase);
        }
    }

    private void DeactivateButton()
    {
        _spawnDroneButton.gameObject.SetActive(false);
    }
}
