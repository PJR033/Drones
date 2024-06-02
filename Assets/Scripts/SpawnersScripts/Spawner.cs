using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Transform ObjectsContainer;
    [SerializeField] protected int MaxObjectsCount;
    [SerializeField] protected bool AutoExpand;
}
