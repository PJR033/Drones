using System;
using System.Collections.Generic;
using UnityEngine;

public class DronesSpawner : Spawner<CollectionDrone>
{
    [SerializeField] private BaseSpawner _baseSpawner;

    private List<Base> _subscribeBases = new List<Base>();

    public event Action<CollectionDrone> DroneSpawned;

    private void OnEnable()
    {
        _baseSpawner.BaseSpawn += SubscribeOnBase;
    }

    private void OnDisable()
    {
        _baseSpawner.BaseSpawn -= SubscribeOnBase;

        foreach (Base subscribeBase in _subscribeBases)
        {
            subscribeBase.SpawnResourcesCollected -= SpawnDrone;
        }
    }

    public void SpawnDrone(Base dronesBase)
    {
        CollectionDrone drone = SpawnMono();
        dronesBase.AddDrone(drone);
        drone.transform.position = dronesBase.transform.position;
        DroneSpawned?.Invoke(drone);
    }

    private void SubscribeOnBase(Base subscribeBase)
    {
        subscribeBase.SpawnResourcesCollected += SpawnDrone;
        _subscribeBases.Add(subscribeBase);
    }
}
