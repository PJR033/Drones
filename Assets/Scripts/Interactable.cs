using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline), typeof(Collider))]
public abstract class Interactable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _outlineWidth = 2.0f;

    private Outline _outline;

    public event Action<Interactable> Selected;

    public bool IsSelected { get; private set; } = false;

    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 0;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        OnHoverExit();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnClick();
    }

    public virtual void OnClick()
    {
        if (IsSelected)
        {
            IsSelected = false;
            OnHoverExit();
        }
        else
        {
            OnHoverEnter();
            _outline.OutlineColor = Color.green;
            IsSelected = true;
            Selected?.Invoke(this);
        }
    }

    private void OnHoverEnter()
    {
        _outline.OutlineWidth = _outlineWidth;
    }

    private void OnHoverExit()
    {
        if (IsSelected == false)
        {
            _outline.OutlineWidth = 0;
            _outline.OutlineColor = Color.white;
        }
    }
}
