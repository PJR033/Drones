using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ObjectPool _crystalsPool;
    [SerializeField] private float _spawnDelay;

    private WaitForSeconds _delay;
    private List<Transform> _spawnPoints = new List<Transform>();

    public event Action<Resource> ResourceSpawned;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _spawnPoints.Add(transform.GetChild(i));
        }

        _delay = new WaitForSeconds(_spawnDelay);
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
                GameObject resource = _crystalsPool.GetObject();
                resource.SetActive(true);
                resource.transform.position = spawnPoint.position;
                ResourceSpawned?.Invoke(resource.GetComponent<Resource>());
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
}
