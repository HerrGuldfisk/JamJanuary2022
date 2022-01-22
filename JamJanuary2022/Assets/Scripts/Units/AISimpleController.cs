using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISimpleController : Unit
{
    public StateMachine stateMachine = new StateMachine();
    public Weapon weapon;
    public Animator anim;

    public Dictionary<AIStates, IState> state = new Dictionary<AIStates, IState>();

    public enum AIStates
    {
        IDLE,
        PATROL,
        PROXIMITY,
        SHOOT,
        DIE
    }

    // Start is called before the first frame update
    void Start()
    {
        state[AIStates.PROXIMITY] = new IProximity(this);
        state[AIStates.DIE] = new IDie(this);
        state[AIStates.SHOOT] = new IShoot(this);

        stateMachine.ChangeState(state[AIStates.PROXIMITY]);
    }

    // Update is called once per frame
    void Update()
    {
        if(stateMachine.currentState != null)
        {
            stateMachine.ExecuteState();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            FindObjectOfType<MapGenerator>().OnKill(transform.position);
            stateMachine.ChangeState(state[AIStates.DIE]);
        }
    }
}
