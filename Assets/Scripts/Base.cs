using System;
using UnityEngine;

[RequireComponent(typeof(ResourceCounter))]
public class Base : Interactable
{
    private int _dronesCount = 0;
    private int _spawnDroneCost = 3;
    private int _buildMinDronesCount = 2;
    private bool _isDroneSendedBuld = false;

    public event Action<Base> TrySpawnFlag;
    public event Action<Crystal> CrystalCollected;
    public event Action<Base> SpawnResourcesCollected;

    public int SpawnBaseCost { get; private set; } = 5;
    public Flag ActiveFlag { get; private set; } = null;
    public ResourceCounter ResourceCounter { get; private set; }
    public ResourceHandler ResourceHandler { get; private set; }
    public bool IsCanBuild => ActiveFlag != null && _dronesCount >= _buildMinDronesCount && ResourceCounter.CrystalsCount >= SpawnBaseCost && _isDroneSendedBuld == false;

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
        ResourceHandler = resourceHandler;
        ResourceCounter = GetComponent<ResourceCounter>();
    }

    public void OnFlagSpawn(Flag flag)
    {
        ActiveFlag = flag;
    }

    public void OnFlagDeactivated()
    {
        ActiveFlag = null;
        _isDroneSendedBuld = false;
    }

    public void OnDroneAdd()
    {
        _dronesCount++;
    }

    public void OnDroneRemove()
    {
        _dronesCount--;
    }

    public void OnDroneSendedBuild()
    {
        _isDroneSendedBuld = true;
    }

    public void TakeResource(Resource resource)
    {
        if (resource is Crystal crystal)
        {
            CrystalCollected?.Invoke(crystal);

            if (ResourceCounter.CrystalsCount >= _spawnDroneCost && ActiveFlag == null)
            {
                SpawnResourcesCollected?.Invoke(this);
                ResourceCounter.DecreaseCrystalsCount(_spawnDroneCost);
            }
        }
    }
}