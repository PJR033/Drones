using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : Spawner
{
    [SerializeField] private Flag _prefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private BaseSpawner _baseSpawner;

    private MonoPool<Flag> _flagsPool;
    private List<Base> _subscribeBases = new List<Base>();
    private Flag _previousFlag;

    public event Action<Flag, Base> FlagSpawned;

    private void Awake()
    {
        _flagsPool = new MonoPool<Flag>(_prefab, MaxObjectsCount, ObjectsContainer, AutoExpand);
    }

    private void OnEnable()
    {
        _baseSpawner.BaseSpawn += SubscribeOnBase;
    }

    private void OnDisable()
    {
        _baseSpawner.BaseSpawn -= SubscribeOnBase;

        foreach (Base subscribedBase in _subscribeBases)
        {
            subscribedBase.TrySpawnFlag -= TrySpawnFlag;
        }
    }

    public void DeactivateFlag(Flag flag)
    {
        _flagsPool.PutElement(flag);
    }

    private void TrySpawnFlag(Base selectedBase)
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.TryGetComponent(out Platform platform))
        {
            if (_previousFlag != null)
            {
                DeactivateFlag(_previousFlag);
            }

            Flag spawnedFlag = SpawnFlag(hit.point, selectedBase);
            selectedBase.OnFlagSpawn(spawnedFlag);
        }
        else if (_previousFlag != null)
        {
            DeactivateFlag(_previousFlag);
        }
    }

    private Flag SpawnFlag(Vector3 flagPosition, Base selectedBase)
    {
        Flag flag = _flagsPool.GetFreeElement();
        flag.transform.position = flagPosition;
        _previousFlag = flag;
        FlagSpawned?.Invoke(flag, selectedBase);
        return flag;
    }

    private void SubscribeOnBase(Base subscribeBase)
    {
        subscribeBase.TrySpawnFlag += TrySpawnFlag;
        _subscribeBases.Add(subscribeBase);
    }
}
