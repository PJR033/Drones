using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;

    private List<Base> _subscribeBases = new List<Base>();

    public event Action<int> CrystalsCountChanged;

    public int CrystalsCount { get; private set; }

    private void OnEnable()
    {
        _baseSpawner.BaseSpawn += SubscribeOnBase;
    }

    private void OnDisable()
    {
        _baseSpawner.BaseSpawn -= SubscribeOnBase;

        foreach (Base subscribeBase in _subscribeBases)
        {
            subscribeBase.CrystalCollected -= IncreaseCrystalsCount;
        }
    }

    public void DecreaseCrystalsCount(int spentCrystalsCount)
    {
        CrystalsCount = Mathf.Clamp(CrystalsCount - spentCrystalsCount, 0, CrystalsCount);
        CrystalsCountChanged?.Invoke(CrystalsCount);
    }

    private void IncreaseCrystalsCount(Crystal crystal)
    {
        CrystalsCount++;
        CrystalsCountChanged?.Invoke(CrystalsCount);
    }

    private void SubscribeOnBase(Base subscribeBase)
    {
        _subscribeBases.Add(subscribeBase);
        subscribeBase.CrystalCollected += IncreaseCrystalsCount;
    }
}
