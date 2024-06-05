using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class ResourceCounter : MonoBehaviour
{
    private Base _collectingBase;

    public event Action<Base> CrystalsCountChanged;

    public int CrystalsCount { get; private set; } = 0;

    private void Start()
    {
        _collectingBase = GetComponent<Base>();
        _collectingBase.CrystalCollected += IncreaseCrystalsCount;
    }

    private void OnDisable()
    {
        _collectingBase = GetComponent<Base>();
        _collectingBase.CrystalCollected -= IncreaseCrystalsCount;
    }

    public void DecreaseCrystalsCount(int spentCrystalsCount)
    {
        CrystalsCount = Mathf.Clamp(CrystalsCount - spentCrystalsCount, 0, CrystalsCount);
        CrystalsCountChanged?.Invoke(_collectingBase);
    }

    private void IncreaseCrystalsCount(Crystal crystal)
    {
        CrystalsCount++;
        CrystalsCountChanged?.Invoke(_collectingBase);
    }
}
