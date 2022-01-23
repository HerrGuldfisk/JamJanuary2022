using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHover : IState
{
    AISimpleController owner;

    public IHover(AISimpleController owner)
    {
        this.owner = owner;
    }

    float bomberTimer = 2f;
    float currentTimer = 0f;

    public void Enter()
    {
        currentTimer = 0;

        // Change to weirder animation?
        owner.anim.Play("Flying");
    }

    public void Execute()
    {
        if(Vector3.Distance(owner.transform.position, PlayerPosition.position) < 12f)
        {
            owner.transform.LookAt(PlayerPosition.position);

            if(currentTimer >= bomberTimer)
            {
                owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.BOMB]);
            }

            currentTimer += Time.deltaTime;
        }
        else
        {
            // Alt. landing
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.REST]);
        }
    }

    public void Exit()
    {
        
    }
}
