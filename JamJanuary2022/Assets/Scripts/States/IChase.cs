using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IChase : IState
{
    AISimpleController owner;

    float unitSpeed = 2f;

    public IChase(AISimpleController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        
    }

    public void Execute()
    {
        owner.character.Move((PlayerPosition.position - owner.transform.position).normalized * Time.deltaTime * unitSpeed);
    }

    public void Exit()
    {
        
    }
}
