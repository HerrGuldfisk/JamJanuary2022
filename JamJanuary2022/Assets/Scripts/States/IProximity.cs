using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IProximity : IState
{
    AISimpleController owner;

    float checkCD = 0.8f;

    float loggedTime;

    public IProximity(AISimpleController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        loggedTime = Time.time;
    }

    public void Execute()
    {
        // Check to ease the cpu usage
        if(Time.time < loggedTime + checkCD) { return; }

        loggedTime += Time.time;
        Collider[] colliders = Physics.OverlapSphere(owner.transform.position, 12f, 10);

        if(colliders.Length == 0) { return; }


        foreach(Collider unit in colliders)
        {
            if(unit.gameObject.tag == "Player")
            {
                owner.transform.LookAt(unit.transform.position, Vector3.up);
                owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.SHOOT]);
            }
        }
    }

    public void Exit()
    {
        
    }
}
