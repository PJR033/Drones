using System;
using UnityEngine;

public class CollectionDrone : MonoBehaviour
{
    [SerializeField] private float _speed;

    private bool _isReady = true;

    public event Action<Vector3, CollectionDrone> BaseBuilt;
    public event Action<Flag> FlagDeactivated;

    public float Speed => _speed;
    public Base Base { get; private set; }
    public Flag ActiveFlag { get; private set; } = null;
    public Resource CollectedResource { get; private set; }

    private void Update()
    {
        if (_isReady)
        {
            if (Base.IsCanBuild)
            {
                ActiveFlag = Base.ActiveFlag;
                Base.OnDroneSendedBuild();
            }
            else if (Base.ResourceHandler.AvailableCrystalsCount > 0)
            {
                CollectedResource = Base.ResourceHandler.GiveResource(Base.transform.position);
            }
        }
    }

    public void Initialize(Base newBase)
    {
        Base = newBase;
    }

    public void OnFlagReached(Flag flag)
    {
        if (Base.ResourceCounter.CrystalsCount >= Base.SpawnBaseCost)
        {
            Base.ResourceCounter.DecreaseCrystalsCount(Base.SpawnBaseCost);
            Base.OnDroneRemove();
            BaseBuilt?.Invoke(flag.transform.position, this);
        }

        DeactivateFlag(flag);
    }

    public void DeactivateFlag(Flag flag)
    {
        FlagDeactivated?.Invoke(flag);
        Base.OnFlagDeactivated();
        ActiveFlag = null;
    }

    public void OnIdleStateEnter()
    {
        _isReady = true;
        ActiveFlag = null;
        CollectedResource = null;
    }

    public void OnIdleStateExit()
    {
        _isReady = false;
    }
}