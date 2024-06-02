using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourceHandler : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _crystalsSpawner;

    private List<Crystal> _crystals = new List<Crystal>();

    public void OnEnable()
    {
        _crystalsSpawner.ResourceSpawned += AddResource;
        _crystalsSpawner.ResourceDeactivated += RemoveResource;
    }

    public void OnDisable()
    {
        _crystalsSpawner.ResourceSpawned -= AddResource;
        _crystalsSpawner.ResourceDeactivated -= RemoveResource;
    }

    public Resource GiveResource(Vector3 basePosition)
    {
        if (_crystals.Count > 0)
        {
            return FindNearlestResource(basePosition);
        }
        else
        {
            return null;
        }
    }

    private Resource FindNearlestResource(Vector3 basePosition)
    {
        Dictionary<float, Resource> distanceCrystals = new Dictionary<float, Resource>();

        foreach (Resource resource in _crystals)
        {
            distanceCrystals.Add(Vector3.Distance(basePosition, resource.transform.position), resource);
        }

        float minDistance = distanceCrystals.Keys.Min();
        Resource nearlestResource = distanceCrystals[minDistance];
        RemoveResource(nearlestResource);
        return nearlestResource;
    }

    private void AddResource(Resource resource)
    {
        if (resource is Crystal crystal)
        {
            _crystals.Add(crystal);
        }
    }

    private void RemoveResource(Resource resource)
    {
        if (resource is Crystal crystal)
        {
            _crystals.Remove(crystal);
        }
    }
}