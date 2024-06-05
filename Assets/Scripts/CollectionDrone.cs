using System;
using System.Collections;
using UnityEngine;

public class CollectionDrone : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Base _base;

    public event Action<Vector3, CollectionDrone> BaseBuilt;
    public event Action<Flag> FlagDeactivated;

    public bool IsReadyToUse { get; private set; } = true;

    public IEnumerator CollectingResource(Resource resource)
    {
        IsReadyToUse = false;
        yield return StartCoroutine(MovingToPosition(resource.transform.position));
        resource.transform.SetParent(transform);
        yield return StartCoroutine(MovingToPosition(_base.transform.position));
        resource.transform.SetParent(null);
        _base.TakeResource(resource);
        IsReadyToUse = true;
    }

    public IEnumerator BuildingBase(Flag flag, ResourceCounter resourceCounter, int spawnBaseCost)
    {
        IsReadyToUse = false;
        yield return StartCoroutine(MovingToPosition(flag.transform.position));

        if (flag.gameObject.activeInHierarchy && transform.position == flag.transform.position)
        {
            if (resourceCounter.CrystalsCount >= spawnBaseCost)
            {
                resourceCounter.DecreaseCrystalsCount(spawnBaseCost);
                BaseBuilt?.Invoke(flag.transform.position, this);
                _base.RemoveDrone(this);
            }

            FlagDeactivated?.Invoke(flag);
            _base.OnFlagDeactivated();
        }

        yield return StartCoroutine(MovingToPosition(_base.transform.position));
        IsReadyToUse = true;
    }

    public void SetBase(Base newBase)
    {
        _base = newBase;
    }

    private IEnumerator MovingToPosition(Vector3 position)
    {
        while (transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}