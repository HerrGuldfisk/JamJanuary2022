using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IChase : IState
{
    AISimpleController owner;

    float unitSpeed = 8f;

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
            Debug.Log($"Moving AI In IChase towards pos: {PlayerPosition.position}");
            owner.character.Move((PlayerPosition.position - owner.transform.position).normalized * Time.deltaTime * unitSpeed);
        }
        
    }

    public void Exit()
    {
        
    }
}
