using System;
using UnityEngine;

public class BaseUIActivator : MonoBehaviour
{
    [SerializeField] ResourceCountViewer _resourceCountViewer;
    [SerializeField] InteractableHandler _interactableHandler;

    public event Action<Base> BaseSelected;
    public event Action<Base> BaseUnselected;

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
            _resourceCountViewer.gameObject.SetActive(true);
            BaseSelected?.Invoke(selectedBase);
        }
    }

    private void DeactivateButton(Interactable interactable)
    {
        if (interactable is Base selectedBase)
        {
            _resourceCountViewer.gameObject.SetActive(false);
            BaseUnselected?.Invoke(selectedBase);
        }
    }
}
