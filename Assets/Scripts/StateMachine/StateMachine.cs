using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StateMachine<T> : MonoBehaviour where T : State
{
    [SerializeField] protected T CurrentState;

    protected Dictionary<Type, T> States = new Dictionary<Type, T>();

    private void Start()
    {
        CurrentState.Enter();
    }

    protected void ChangeState<U>() where U : T
    {
        Type type = typeof(U);
        CurrentState.Exit();
        CurrentState = States[type];
        CurrentState.Enter();
    }
}
