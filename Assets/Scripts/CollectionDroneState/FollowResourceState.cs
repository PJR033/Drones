using UnityEngine;

public class FollowResourceState : FollowState
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Crystal crystal) && crystal == CollectionDrone.CollectedResource)
        {
            CollectionDrone.CollectedResource.transform.SetParent(transform);
            DroneStateMachine.StartFollowBase();
        }
    }

    public override void Enter()
    {
        base.Enter();
        FollowPosition = CollectionDrone.CollectedResource.transform.position;
    }
}
