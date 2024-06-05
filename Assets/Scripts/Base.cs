using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ResourceCounter))]
public class Base : Interactable
{
    private ResourceHandler _resourceHandler;
    private List<CollectionDrone> _drones = new List<CollectionDrone>();
    private CollectionDrone _readyDrone = null;
    private bool _isCanSendBuild = false;
    private int _spawnBaseCost = 5;
    private int _spawnDroneCost = 3;
    private int _impossibilitySpawnDronesMaxCount = 1;

    public event Action<Base> TrySpawnFlag;
    public event Action<Crystal> CrystalCollected;
    public event Action<Base> SpawnResourcesCollected;

    public Flag ActiveFlag { get; private set; } = null;
    public ResourceCounter ResourceCounter { get; private set; }

    private void Start()
    {
        StartCoroutine(FindingReadyDrone());
    }

    private void Update()
    {
        if (_readyDrone != null)
        {
            if (_isCanSendBuild && ResourceCounter.CrystalsCount >= _spawnBaseCost && _drones.Count > _impossibilitySpawnDronesMaxCount)
            {
                SendDroneBuild();
            }
            else if (_resourceHandler.AvailableCrystalsCount > 0)
            {
                SendDroneCollect();
            }
        }
    }

    public override void OnClick()
    {
        if (IsSelected)
        {
            TrySpawnFlag?.Invoke(this);
        }

        base.OnClick();
    }

    public void Initialize(ResourceHandler resourceHandler)
    {
        _resourceHandler = resourceHandler;
        ResourceCounter = GetComponent<ResourceCounter>();
    }

    public void OnFlagSpawn(Flag flag)
    {
        ActiveFlag = flag;
        _isCanSendBuild = true;
    }

    public void OnFlagDeactivated()
    {
        ActiveFlag = null;
    }

    public void RemoveDrone(CollectionDrone removedDrone)
    {
        _drones.Remove(removedDrone);
    }

    public void AddDrone(CollectionDrone addedDrone)
    {
        _drones.Add(addedDrone);
        addedDrone.SetBase(this);
    }

    public void TakeResource(Resource resource)
    {
        if (resource is Crystal)
        {
            CrystalCollected?.Invoke((Crystal)resource);

            if (ResourceCounter.CrystalsCount >= _spawnDroneCost && ActiveFlag == null)
            {
                SpawnResourcesCollected?.Invoke(this);
                ResourceCounter.DecreaseCrystalsCount(_spawnDroneCost);
            }
        }
    }

    private void SendDroneCollect()
    {
        Resource resource = _resourceHandler.GiveResource(transform.position);

        if (resource != null)
        {
            StartCoroutine(_readyDrone.CollectingResource(resource));
            _readyDrone = null;
        }
    }

    private void SendDroneBuild()
    {
        StartCoroutine(_readyDrone.BuildingBase(ActiveFlag, ResourceCounter, _spawnBaseCost));
        _readyDrone = null;
        _isCanSendBuild = false;
    }

    private IEnumerator FindingReadyDrone()
    {
        bool isCanFind = true;

        while (isCanFind)
        {
            if (_readyDrone == null && _drones.Count > 0)
            {
                foreach (CollectionDrone drone in _drones)
                {
                    if (drone.IsReadyToUse)
                    {
                        _readyDrone = drone;
                    }
                }
            }

            yield return null;
        }
    }
}