using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseSpawner : Spawner
{
    [SerializeField] private Transform _startBaseSpawnPoint;
    [SerializeField] private Base _prefab;
    [SerializeField] private DronesSpawner _dronesSpawner;
    [SerializeField] private ResourceSpawner _crystalsSpawner;
    [SerializeField] private FlagSpawner _flagSpawner;
    [SerializeField] private Transform _dronesContainer;
    [SerializeField] private ResourceHandler _resourceHandler;
    [SerializeField] private ResourceCounter _resourceCounter;

    private MonoPool<Base> _basesPool;
    private List<CollectionDrone> _subscribeDrone = new List<CollectionDrone>();

    public event Action<Base> BaseSpawn;

    private void Start()
    {
        _basesPool = new MonoPool<Base>(_prefab, MaxObjectsCount, ObjectsContainer, AutoExpand);
        CollectionDrone startDrone = _dronesContainer.GetChild(0).GetComponent<CollectionDrone>();
        SubscribeOnDrone(startDrone);
        SpawnBase(_startBaseSpawnPoint.position, startDrone);
    }

    private void OnEnable()
    {
        _dronesSpawner.DroneSpawn += SubscribeOnDrone;
    }

    private void OnDisable()
    {
        foreach (CollectionDrone drone in _subscribeDrone)
        {
            drone.BaseBuilt -= SpawnBase;
            drone.FlagDeactivated -= _flagSpawner.DeactivateFlag;
        }
    }

    private void SpawnBase(Vector3 flagPosition, CollectionDrone buildingDrone)
    {
        Base newBase = _basesPool.GetFreeElement();
        newBase.Initialize(_resourceHandler, _resourceCounter);
        newBase.AddDrone(buildingDrone);
        Vector3 spawnBasePosition = new Vector3(flagPosition.x, _startBaseSpawnPoint.position.y, flagPosition.z);
        newBase.transform.position = spawnBasePosition;
        BaseSpawn?.Invoke(newBase);
    }

    private void SubscribeOnDrone(CollectionDrone drone)
    {
        _subscribeDrone.Add(drone);
        drone.BaseBuilt += SpawnBase;
        drone.FlagDeactivated += _flagSpawner.DeactivateFlag;
    }
}
