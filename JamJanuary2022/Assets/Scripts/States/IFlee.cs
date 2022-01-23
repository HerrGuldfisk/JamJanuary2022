using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFlee : IState
{
    AISimpleController owner;

    public IFlee(AISimpleController owner)
    {
        this.owner = owner;
    }

    float unitSpeed = 5f;

    float fleeTimer = 2f;
    float currentTime;

    public void Enter()
    {
        currentTime = fleeTimer;
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
            owner.transform.LookAt(owner.transform.position - PlayerPosition.position);
            owner.character.Move((owner.transform.position - PlayerPosition.position).normalized * Time.deltaTime * unitSpeed);

            currentTime -= Time.deltaTime;
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
