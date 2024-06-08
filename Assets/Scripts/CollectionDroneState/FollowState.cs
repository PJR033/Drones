using UnityEngine;

public abstract class FollowState : DroneState
{
    protected Vector3 FollowPosition;

    protected virtual void Update() => Move();

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, FollowPosition, CollectionDrone.Speed * Time.deltaTime);
    }

    public void SetFollowPosition(Vector3 followPosition)
    {
        FollowPosition = followPosition;
    }
}