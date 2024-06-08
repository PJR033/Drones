using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected virtual void Awake()
    {
        enabled = false;
    }

    public virtual void Enter() =>
        enabled = true;

    public virtual void Exit()
    {
        enabled = false;
    }
}
