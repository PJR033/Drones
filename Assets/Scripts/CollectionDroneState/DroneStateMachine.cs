using UnityEngine;

[RequireComponent(typeof(FollowBaseState))]
[RequireComponent(typeof(FollowResourceState))]
[RequireComponent(typeof(FollowFlagState))]
[RequireComponent(typeof(DroneIdleState))]
public class DroneStateMachine : StateMachine<DroneState>
{
    private void Awake()
    {
        States.Add(typeof(FollowBaseState), GetComponent<FollowBaseState>());
        States.Add(typeof(FollowResourceState), GetComponent<FollowResourceState>());
        States.Add(typeof(FollowFlagState), GetComponent<FollowFlagState>());
        States.Add(typeof(DroneIdleState), GetComponent<DroneIdleState>());
    }

    public void StartFollowBase()
    {
        ChangeState<FollowBaseState>();
    }

    public void StartFollowResource()
    {
        ChangeState<FollowResourceState>();
    }

    public void StartFollowFlag()
    {
        ChangeState<FollowFlagState>();
    }

    public void StartIdleState()
    {
        ChangeState<DroneIdleState>();
    }
}
