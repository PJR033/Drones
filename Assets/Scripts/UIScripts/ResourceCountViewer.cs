using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourceCountViewer : MonoBehaviour
{
    [SerializeField] private BaseUIActivator _baseUIActivator;

    private TextMeshProUGUI _text;
    private string _counterHeaderText = "Кристаллов: ";

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _baseUIActivator.BaseSelected += ViewResourceCount;
        _baseUIActivator.BaseUnselected += UnsubscribeCounter;
    }

    private void OnDisable()
    {
        _baseUIActivator.BaseSelected -= ViewResourceCount;
        _baseUIActivator.BaseUnselected -= UnsubscribeCounter;
    }

    private void ViewResourceCount(Base selectedBase)
    {
        string counterValueText = selectedBase.ResourceCounter.CrystalsCount.ToString();
        _text.text = _counterHeaderText + counterValueText;
        selectedBase.ResourceCounter.CrystalsCountChanged += ViewResourceCount;
    }

    private void UnsubscribeCounter(Base unselectedBase)
    {
        unselectedBase.ResourceCounter.CrystalsCountChanged -= ViewResourceCount;
    }
}
