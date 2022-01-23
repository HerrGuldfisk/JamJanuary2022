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

    float unitSpeed = 2.5f;

    float maxTime = 4f;
    float currentTime;

    public void Enter()
    {
        currentTime = maxTime;
        owner.anim.Play("Idle");
    }

    public void Execute()
    {
        RaycastHit hit;
        Ray downRay = new Ray(owner.transform.position - new Vector3(0, -1, 0), Vector3.down);

        if (Physics.Raycast(downRay, out hit, 50f))
        {
            if (hit.distance > 0.1f)
            {
                owner.character.Move(new Vector3(0, -9.82f, 0) * Time.deltaTime);
            }
        }

        if (PlayerPosition.position != null && currentTime > 0)
        {
            owner.character.Move((PlayerPosition.position - owner.transform.position).normalized * Time.deltaTime * unitSpeed);
            owner.character.Move(new Vector3(0, 1, 0) * Time.deltaTime);
            owner.transform.LookAt(PlayerPosition.position);

            currentTime -= Time.deltaTime;
        }
        else if (currentTime < 0 && Vector3.Distance(PlayerPosition.position, owner.transform.position) > 16f)
        {
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.CHASEAIR]);
        }
        else
        {
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.REST]);
        }
    }

    public void Exit()
    {
        
    }
}
