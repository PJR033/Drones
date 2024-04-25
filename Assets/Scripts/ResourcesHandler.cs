using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourcesHandler : MonoBehaviour
{
    [SerializeField] private ObjectPool _crystalsPool;
    [SerializeField] private ResourceSpawner _crystalsSpawner;

    private Dictionary<float, Resource> _crystals = new Dictionary<float, Resource>();

    public int AvailableCrystalsCount { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < _crystalsPool.transform.childCount; i++)
        {
            _crystals.Add(Vector3.Distance(transform.position, _crystalsPool.transform.GetChild(i).transform.position), _crystalsPool.transform.GetChild(i).GetComponent<Crystal>());
        }
    }

    private void OnEnable()
    {
        _crystalsSpawner.ResourceSpawned += AddResource;
    }

    private void OnDisable()
    {
        _crystalsSpawner.ResourceSpawned -= AddResource;
    }

    public Resource FindNearlestResource()
    {
        if (_crystals.Count > 0)
        {
            float minDistance = _crystals.Keys.Min();
            Resource nearlestResource = _crystals[minDistance];
            _crystals.Remove(_crystals.Keys.Min());
            AvailableCrystalsCount = _crystals.Count;
            return nearlestResource;
        }
        else
        {
            return null;
        }
    }

    private void AddResource(Resource resource)
    {
        if (resource is Crystal)
        {
            _crystals.Add(Vector3.Distance(transform.position, resource.transform.position), resource);
        }

        AvailableCrystalsCount = _crystals.Count;
    }
}