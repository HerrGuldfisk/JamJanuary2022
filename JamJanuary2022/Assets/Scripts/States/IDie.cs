using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDie : IState
{
    Unit owner;
    [SerializeField] GameObject sfxPop;
    [SerializeField] GameObject vfxPop;

    public IDie(Unit owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        GameObject.Instantiate(sfxPop, owner.transform.position, Quaternion.identity);
        GameObject.Instantiate(vfxPop, owner.transform.position, Quaternion.identity);
        GameObject.FindObjectOfType<ScoreSystem>().Add(1);   
        owner.DestroyUnit();
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
        
    }
}
