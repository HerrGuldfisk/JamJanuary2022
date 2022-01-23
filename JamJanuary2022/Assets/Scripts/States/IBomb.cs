using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBomb : IState
{
    AISimpleController owner;

    public IBomb(AISimpleController owner)
    {
        this.owner = owner;
    }

    Vector3 startPos;
    Vector3 targetPos;
    float pos;

    float bombSpeed = 6f;

    public void Enter()
    {
        startPos = owner.transform.position;
        targetPos = PlayerPosition.position;
        pos = 0;
    }

    public void Execute()
    {
        owner.character.Move((targetPos - startPos).normalized * Time.deltaTime * bombSpeed);

        if(Vector3.Distance(owner.transform.position, targetPos) < 0.1f)
        {
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.DIE]);
        }
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}