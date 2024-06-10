using UnityEngine;

public class FollowBaseState : FollowState
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Base dronesBase) && dronesBase == CollectionDrone.Base)
        {
            if (CollectionDrone.CollectedResource != null)
            {
                CollectionDrone.CollectedResource.transform.SetParent(null);
                CollectionDrone.Base.TakeResource(CollectionDrone.CollectedResource);
            }

            DroneStateMachine.StartIdleState();
        }
    }

    public override void Enter()
    {
        base.Enter();
        FollowPosition = CollectionDrone.Base.transform.position;
    }
}
