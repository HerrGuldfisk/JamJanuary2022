using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRest : IState
{
    AISimpleController owner;

    public IRest(AISimpleController owner)
    {
        this.owner = owner;
    }

    float unitSpeed = 1.5f;

    float maxTime = 4f;
    float currentTime;

    public void Enter()
    {
        currentTime = maxTime;
        owner.anim.Play("Idle");
    }

    public void Execute()
    {
        if (PlayerPosition.position != null && currentTime > 0)
        {
            owner.character.Move((PlayerPosition.position - owner.transform.position).normalized * Time.deltaTime * unitSpeed);
            owner.character.Move(new Vector3(0, 1, 0) * Time.deltaTime);
            owner.transform.LookAt(PlayerPosition.position);

            currentTime -= Time.deltaTime;
        }
        else if (currentTime < 0)
        {
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.CHASEAIR]);
        }
    }

    public void Exit()
    {
        
    }
}
