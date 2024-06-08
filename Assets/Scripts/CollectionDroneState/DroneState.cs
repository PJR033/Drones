using UnityEngine;

[RequireComponent(typeof(DroneStateMachine), typeof(CollectionDrone))]
public abstract class DroneState : State
{
    protected DroneStateMachine DroneStateMachine;
    protected CollectionDrone CollectionDrone;

    protected override void Awake()
    {
        base.Awake();
        DroneStateMachine = GetComponent<DroneStateMachine>();
        CollectionDrone = GetComponent<CollectionDrone>();
    }
}
