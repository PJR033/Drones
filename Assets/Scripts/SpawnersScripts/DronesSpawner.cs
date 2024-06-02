using System;
using UnityEngine;

public class DronesSpawner : Spawner
{
    [SerializeField] private CollectionDrone _prefab;
    [SerializeField] private SpawnDroneButton _spawnDroneButton;

    private MonoPool<CollectionDrone> _dronesPool;

    public event Action<CollectionDrone> DroneSpawn;

    private void Awake()
    {
        _dronesPool = new MonoPool<CollectionDrone>(_prefab, MaxObjectsCount, ObjectsContainer, AutoExpand);
    }

    private void OnEnable()
    {
        _spawnDroneButton.SpawnDrone += SpawnDrone;
    }

    private void OnDisable()
    {
        _spawnDroneButton.SpawnDrone -= SpawnDrone;
    }

    public void SpawnDrone(Base dronesBase)
    {
        CollectionDrone drone = _dronesPool.GetFreeElement();
        dronesBase.AddDrone(drone);
        drone.transform.position = dronesBase.transform.position;
        DroneSpawn(drone);
    }
}
