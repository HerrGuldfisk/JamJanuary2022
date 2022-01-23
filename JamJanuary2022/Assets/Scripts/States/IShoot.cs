using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IShoot : IState
{
    AISimpleController owner;
    Weapon weapon;

    float shootCD = 0.5f;
    float reloadCD = 2f;

    const int MAXBULLETS = 3;
    int bullets = 3;

    float currentTimer = 0f;

    public IShoot(AISimpleController owner)
    {
        this.owner = owner;
        this.weapon = owner.weapon;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        if(bullets > 0 && currentTimer <= 0)
        {
            RaycastHit hit;
            Ray forwardRay = new Ray(owner.transform.position, Vector3.forward);

            if (Physics.Raycast(forwardRay, out hit, 50f))
            {
                //GameObject tempBullet = weapon.SpawnBullet();
            }
        }
    }

    public void Exit()
    {
        
    }
}
