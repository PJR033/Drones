using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourceCountViewer : MonoBehaviour
{
    [SerializeField] private ResourceCounter _counter;

    private TextMeshProUGUI _text;
    private string _counterHeaderText;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _counterHeaderText = "Кристаллов: ";
    }

    private void OnEnable()
    {
        _counter.CrystalsCountChanged += ViewResourceCount;
    }

    private void OnDisable()
    {
        _counter.CrystalsCountChanged -= ViewResourceCount;
    }

    private void ViewResourceCount(int crystalsCount)
    {
        string counterValueText = crystalsCount.ToString();
        _text.text = _counterHeaderText + counterValueText;
    }
}
