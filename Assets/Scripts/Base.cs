using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ResourceCounter), (typeof(ResourcesHandler)))]
public class Base : MonoBehaviour
{
    [SerializeField] private ObjectPool _dronesPool;
    [SerializeField] private ObjectPool _crystalsPool;

    private List<CollectionDrone> _drones = new List<CollectionDrone>();
    private ResourceCounter _crystalsCounter;
    private ResourcesHandler _resourcesHandler;

    private void Awake()
    {
        _resourcesHandler = GetComponent<ResourcesHandler>();
        _crystalsCounter = GetComponent<ResourceCounter>();

        for (int i = 0; i < _dronesPool.transform.childCount; i++)
        {
            _drones.Add(_dronesPool.transform.GetChild(i).GetComponent<CollectionDrone>());
        }
    }

    private void Update()
    {
        SendDrone();
    }

    public void TakeResource(Resource resource)
    {
        if (resource is Crystal)
        {
            _crystalsPool.PutObject(resource.gameObject);
            _crystalsCounter.IncreaseCrystalsCount();
        }
    }

    private void SendDrone()
    {
        if (_resourcesHandler.AvailableCrystalsCount > 0)
        {
            FindReadyDrone();
        }
    }

    private void FindReadyDrone()
    {
        foreach (CollectionDrone drone in _drones)
        {
            if (drone.IsReadyToUse)
            {
                Resource nearlestResource = _resourcesHandler.FindNearlestResource();

                if (nearlestResource != null)
                {
                    StartCoroutine(drone.CollectingResource(nearlestResource));
                    break;
                }
            }
        }
    }
}
