public class DroneIdleState : DroneState
{
    private void Update()
    {
        if (CollectionDrone.ActiveFlag != null)
        {
            CollectionDrone.OnIdleStateExit();
            DroneStateMachine.StartFollowFlag();
        }
        else if (CollectionDrone.CollectedResource != null)
        {
            CollectionDrone.OnIdleStateExit();
            DroneStateMachine.StartFollowResource();
        }
    }

    public override void Enter()
    {
        base.Enter();
        CollectionDrone.OnIdleStateEnter();
    }
}
