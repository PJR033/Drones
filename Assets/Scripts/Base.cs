using System;
using System.Collections.Generic;
using System.Collections;

public class Base : Interactable
{
    private ResourceCounter _resourceCounter;
    private ResourceHandler _resourceHandler;
    private List<CollectionDrone> _drones = new List<CollectionDrone>();
    private CollectionDrone _readyDrone = null;
    private Flag _activeFlag = null;
    private int _spawnBaseCost = 5;

    public event Action<Base> TrySpawnFlag;
    public event Action<Crystal> CrystalCollected;

    private void Start()
    {
        StartCoroutine(FindingReadyDrone());
    }

    private void Update()
    {
        if (_readyDrone != null)
        {
            if (_activeFlag != null && _resourceCounter.CrystalsCount >= _spawnBaseCost)
            {
                SendDroneBuild();
            }
            else
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

    public void Initialize(ResourceHandler resourceHandler, ResourceCounter resourceCounter)
    {
        _resourceCounter = resourceCounter;
        _resourceHandler = resourceHandler;
    }

    public void OnFlagSpawn(Flag flag)
    {
        _activeFlag = flag;
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
        StartCoroutine(_readyDrone.BuildingBase(_activeFlag, _resourceCounter, _spawnBaseCost));
        _readyDrone = null;
        _activeFlag = null;
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