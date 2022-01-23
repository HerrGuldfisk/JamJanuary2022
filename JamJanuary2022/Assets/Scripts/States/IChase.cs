using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IChase : IState
{
    AISimpleController owner;

    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 midPosition;

    float currentTime;

    float count
    {
        get
        {
            return currentTime / 4f;
        }
    }

    public IChase(AISimpleController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        currentTime = 0;
        owner.anim.Play("Flying");

        startPosition = owner.transform.position;

        Vector3 playerPos = new Vector3(PlayerPosition.position.x, 0, PlayerPosition.position.z);
        Vector3 ownerPos = new Vector3(owner.transform.position.x, 0, owner.transform.position.z);

        if(Vector3.Distance(PlayerPosition.position, owner.transform.position) > 12f)
        {
            endPosition = owner.transform.position + (playerPos - ownerPos).normalized * 8f;
        }
        else
        {
            endPosition = PlayerPosition.position;
        }
        
        // endPosition = PlayerPosition.position;
        midPosition = startPosition + (endPosition - startPosition) / 2f + Vector3.up * Random.Range(3f, 7f)
            + new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

        //Debug.Log(startPosition);
        //Debug.Log(endPosition);
        //Debug.Log(midPosition);
    }

    public void Execute()
    {
        if (PlayerPosition.position != null)
        {
            if (count < 1.0f)
            {
                currentTime += Time.deltaTime;

                Vector3 m1 = Vector3.Lerp(startPosition, midPosition, count);
                Vector3 m2 = Vector3.Lerp(midPosition, endPosition, count);
                owner.character.Move(Vector3.Lerp(m1, m2, count) - owner.transform.position);

                if(Vector3.Distance(owner.transform.position, PlayerPosition.position) < 7f)
                {
                    Debug.Log("Enter hover");
                    owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.HOVER]);
                }
            }
            else
            {
                owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.REST]);
            }
            // owner.character.Move((PlayerPosition.position - owner.transform.position).normalized * Time.deltaTime * unitSpeed);
            owner.transform.LookAt(PlayerPosition.position);
        }

        /*
        if(Vector3.Distance(owner.transform.position, PlayerPosition.position) < 10f)
        {
            owner.stateMachine.ChangeState(owner.state[AISimpleController.AIStates.SHOOT]);
        }
        */
    }

    public void Exit()
    {

    }

    
}
