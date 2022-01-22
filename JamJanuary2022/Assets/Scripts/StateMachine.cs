using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState currentState;
    public IState previousState;

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        previousState = currentState;
        currentState = newState;
        currentState.Enter();
    }

    public void ExecuteState()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }

    public void PreviousState()
    {
        if (previousState != null)
        {
            currentState.Exit();
            IState tempState = previousState;
            previousState = currentState;
            currentState = tempState;
            currentState.Enter();
        }
        else
        {
            Debug.Log("Previous state does not exist");
        }
    }
}
