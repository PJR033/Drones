using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    private int _crystalsCount;

    public event Action<int> CrystalsCountChanged;

    public void IncreaseCrystalsCount()
    {
        _crystalsCount++;
        CrystalsCountChanged?.Invoke(_crystalsCount);
    }
}
