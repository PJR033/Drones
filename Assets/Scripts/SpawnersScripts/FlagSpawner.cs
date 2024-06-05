using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : Spawner<Flag>
{
    [SerializeField] private Camera _camera;
    [SerializeField] private BaseSpawner _baseSpawner;

    private List<Base> _subscribeBases = new List<Base>();

    private void OnEnable()
    {
        _baseSpawner.BaseSpawn += SubscribeOnBase;
    }

    private void OnDisable()
    {
        _baseSpawner.BaseSpawn -= SubscribeOnBase;

        foreach (Base subscribedBase in _subscribeBases)
        {
            subscribedBase.TrySpawnFlag -= TrySpawnFlag;
        }
    }

    public void DeactivateFlag(Flag flag)
    {
        DeactivateMono(flag);
    }

    private void TrySpawnFlag(Base selectedBase)
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.TryGetComponent(out Platform platform))
        {
            if (selectedBase.ActiveFlag != null)
            {
                DeactivateFlag(selectedBase.ActiveFlag);
                selectedBase.OnFlagDeactivated();
            }

            Flag spawnedFlag = SpawnFlag(hit.point, selectedBase);
            selectedBase.OnFlagSpawn(spawnedFlag);
        }
        else if (selectedBase.ActiveFlag != null)
        {
            DeactivateFlag(selectedBase.ActiveFlag);
            selectedBase.OnFlagDeactivated();
        }
    }

    private Flag SpawnFlag(Vector3 flagPosition, Base selectedBase)
    {
        Flag flag = SpawnMono();
        flag.transform.position = flagPosition;
        return flag;
    }

    private void SubscribeOnBase(Base subscribeBase)
    {
        subscribeBase.TrySpawnFlag += TrySpawnFlag;
        _subscribeBases.Add(subscribeBase);
    }
}
