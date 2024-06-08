public class FollowResourceState : FollowState
{
    protected override void Update()
    {
        base.Update();

        if (transform.position == FollowPosition)
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
