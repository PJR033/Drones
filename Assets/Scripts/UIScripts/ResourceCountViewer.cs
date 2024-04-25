using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourceCountViewer : MonoBehaviour
{
    [SerializeField] private ResourceCounter _counter;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
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
        string crystalsText = "Кристаллов: ";
        _text.text = crystalsText + crystalsCount.ToString();
    }
}
