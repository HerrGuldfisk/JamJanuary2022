using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IChase : IState
{
    AISimpleController owner;

    float unitSpeed = 3f;

    public IChase(AISimpleController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        if(PlayerPosition.position != null)
        {
            owner.character.Move((PlayerPosition.position - owner.transform.position).normalized * Time.deltaTime * unitSpeed);
            owner.transform.LookAt(PlayerPosition.position);
        }

        if(Vector3.Distance(owner.transform.position, PlayerPosition.position) < 10f)
        {
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.SHOOT]);
        }
        
    }

    public void Exit()
    {
        
    }
}
