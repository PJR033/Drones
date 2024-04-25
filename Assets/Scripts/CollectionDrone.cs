using System.Collections;
using UnityEngine;

public class CollectionDrone : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Base _base;

    public bool IsReadyToUse { get; private set; } = true;

    public IEnumerator CollectingResource(Resource resource)
    {
        IsReadyToUse = false;

        while (transform.position != resource.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, resource.transform.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        resource.transform.SetParent(transform);

        while (transform.position != _base.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _base.transform.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        resource.transform.SetParent(null);
        _base.TakeResource(resource);
        IsReadyToUse = true;
    }
}