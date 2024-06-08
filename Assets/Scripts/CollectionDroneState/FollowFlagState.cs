public class FollowFlagState : FollowState
{
    protected override void Update()
    {
        if (CollectionDrone.ActiveFlag.gameObject.activeInHierarchy)
        {
            base.Update();
        }
        else
        {
            CollectionDrone.DeactivateFlag(CollectionDrone.ActiveFlag);
            DroneStateMachine.StartFollowBase();
        }

        if (transform.position == FollowPosition && CollectionDrone.ActiveFlag.gameObject.activeInHierarchy)
        {
            CollectionDrone.OnFlagReached(CollectionDrone.ActiveFlag);
            DroneStateMachine.StartFollowBase();
        }
    }

    public override void Enter()
    {
        base.Enter();
        FollowPosition = CollectionDrone.ActiveFlag.transform.position;
    }
}
