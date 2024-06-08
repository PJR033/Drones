using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : Spawner<Base>
{
    [SerializeField] private Transform _startBaseSpawnPoint;
    [SerializeField] private DronesSpawner _dronesSpawner;
    [SerializeField] private FlagSpawner _flagSpawner;
    [SerializeField] private Transform _dronesContainer;
    [SerializeField] private ResourceHandler _resourceHandler;

    private List<CollectionDrone> _subscribeDrone = new List<CollectionDrone>();

    public event Action<Base> BaseSpawn;

    private void Start()
    {
        CollectionDrone startDrone = _dronesContainer.GetChild(0).GetComponent<CollectionDrone>();
        SubscribeOnDrone(startDrone);
        SpawnBase(_startBaseSpawnPoint.position, startDrone);
    }

    private void OnEnable()
    {
        _dronesSpawner.DroneSpawned += SubscribeOnDrone;
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
        Base newBase = SpawnMono();
        newBase.Initialize(_resourceHandler);
        newBase.OnDroneAdd();
        buildingDrone.Initialize(newBase);
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
