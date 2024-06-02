using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceSpawner : Spawner
{
    [SerializeField] Resource _resourcePrefab;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private BaseSpawner _baseSpawner;

    private MonoPool<Resource> _resourcesPool;
    private WaitForSeconds _delay;
    private List<Transform> _spawnPoints = new List<Transform>();
    private List<Base> _subscribeBases = new List<Base>();

    public event Action<Resource> ResourceSpawned;
    public event Action<Resource> ResourceDeactivated;

    private void Awake()
    {
        _resourcesPool = new MonoPool<Resource>(_resourcePrefab, MaxObjectsCount, ObjectsContainer, AutoExpand);

        for (int i = 0; i < transform.childCount; i++)
        {
            _spawnPoints.Add(transform.GetChild(i));
        }

        _delay = new WaitForSeconds(_spawnDelay);
    }

    private void OnEnable()
    {
        _baseSpawner.BaseSpawn += SubscribeOnBase;
    }

    private void OnDisable()
    {
        _baseSpawner.BaseSpawn -= SubscribeOnBase;

        foreach (Base subscribeBase in _subscribeBases)
        {
            subscribeBase.CrystalCollected -= DeactivateResource;
        }
    }

    private void Start()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        bool isCanSpawn = true;

        yield return _delay;

        while (isCanSpawn)
        {
            Transform spawnPoint = ChooseSpawnPoint();

            if (spawnPoint != null)
            {
                Resource resource = _resourcesPool.GetFreeElement();
                resource.transform.position = spawnPoint.position;
                ResourceSpawned?.Invoke(resource);
                yield return _delay;
            }
            else
            {
                isCanSpawn = false;
            }
        }
    }

    private Transform ChooseSpawnPoint()
    {
        if (_spawnPoints.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _spawnPoints.Count);
            Transform spawnPoint = SetRandomPointPosition(_spawnPoints[index]);
            return spawnPoint;
        }
        else
        {
            return null;
        }
    }

    private Transform SetRandomPointPosition(Transform spawnPoint)
    {
        float maxOffset = 0.5f;
        float minOffset = -0.5f;
        Transform randomizedTransform = spawnPoint;
        Vector3 randomizedVector = new Vector3(randomizedTransform.position.x + UnityEngine.Random.Range(minOffset, maxOffset), randomizedTransform.position.y, randomizedTransform.position.z + UnityEngine.Random.Range(minOffset, maxOffset));
        randomizedTransform.position = randomizedVector;
        return randomizedTransform;
    }

    private void SubscribeOnBase(Base subscribeBase)
    {
        _subscribeBases.Add(subscribeBase);
        subscribeBase.CrystalCollected += DeactivateResource;
    }

    private void DeactivateResource(Resource resource)
    {
        _resourcesPool.PutElement(resource);
        ResourceDeactivated?.Invoke(resource);
    }
}
