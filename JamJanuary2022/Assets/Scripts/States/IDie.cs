using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDie : IState
{
    Unit owner;

    public IDie(Unit owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.DestroyUnit();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
