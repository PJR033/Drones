using System;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Transform ObjectsContainer;
    [SerializeField] private int MaxObjectsCount;
    [SerializeField] private bool AutoExpand;
    [SerializeField] private T _prefab;

    private MonoPool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new MonoPool<T>(_prefab, MaxObjectsCount, ObjectsContainer, AutoExpand);
    }

    protected T SpawnMono()
    {
        T mono = _pool.GetFreeElement();
        return mono;
    }

    protected void DeactivateMono(T mono)
    {
        _pool.PutElement(mono);
    }
}
