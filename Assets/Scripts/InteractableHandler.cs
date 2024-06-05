using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractableHandler : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;

    private Interactable _selectedInteractable;
    private List<Base> _subscribeBases = new List<Base>();
    private bool _isButtonHovered = false;

    public event Action<Interactable> InteractableSelected;
    public event Action<Interactable> InteractableUnselected;

    private void OnEnable()
    {
        _baseSpawner.BaseSpawn += SubscribeOnBase;
    }

    private void OnDisable()
    {
        _baseSpawner.BaseSpawn -= SubscribeOnBase;

        foreach (Base subscribeBase in _subscribeBases)
        {
            subscribeBase.Selected -= SetSelectedInteractable;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _selectedInteractable != null)
        {
            if (_selectedInteractable.IsSelected && _isButtonHovered == false)
            {
                _selectedInteractable.OnClick();
                InteractableUnselected?.Invoke(_selectedInteractable);
                _selectedInteractable = null;
            }
        }
    }

    private void SubscribeOnBase(Base subscribeBase)
    {
        subscribeBase.Selected += SetSelectedInteractable;
    }

    private void SetSelectedInteractable(Interactable selectedInteractable)
    {
        _selectedInteractable = selectedInteractable;
        InteractableSelected?.Invoke(selectedInteractable);
    }
}