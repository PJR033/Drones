using UnityEngine;

public class FollowFlagState : FollowState
{
    protected override void Update()
    {
        if (CollectionDrone.ActiveFlag.enabled)
        {
            base.Update();
        }
        else
        {
            CollectionDrone.DeactivateFlag(CollectionDrone.ActiveFlag);
            DroneStateMachine.StartFollowBase();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Flag flag) && flag == CollectionDrone.ActiveFlag)
        {
            if (CollectionDrone.ActiveFlag.enabled)
            {
                Debug.Log("Флаг достигнут");
                CollectionDrone.OnFlagReached(CollectionDrone.ActiveFlag);
                DroneStateMachine.StartFollowBase();
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        FollowPosition = CollectionDrone.ActiveFlag.transform.position;
    }
}
