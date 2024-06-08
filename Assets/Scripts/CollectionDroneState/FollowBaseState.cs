public class FollowBaseState : FollowState
{
    protected override void Update()
    {
        base.Update();

        if (transform.position == FollowPosition)
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
