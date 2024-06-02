using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class SpawnDroneButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpawnDroneButtonActivator _activator;
    [SerializeField] private ResourceCounter _counter;
    [SerializeField] private int _spawnDroneCost;

    private Button _button;
    private Base _dronesBase;

    public event Action<Base> SpawnDrone;
    public event Action PointEnter;
    public event Action PointExit;

    private void Awake()
    {
        _button = GetComponent<Button>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _activator.BaseSelected += SetDronesBase;
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _activator.BaseSelected -= SetDronesBase;
        _button.onClick.RemoveListener(OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointExit?.Invoke();
    }

    private void SetDronesBase(Base dronesBase)
    {
        _dronesBase = dronesBase;
    }

    private void OnClick()
    {
        if (_counter.CrystalsCount >= _spawnDroneCost)
        {
            _counter.DecreaseCrystalsCount(_spawnDroneCost);
            SpawnDrone?.Invoke(_dronesBase);
        }
    }
}
