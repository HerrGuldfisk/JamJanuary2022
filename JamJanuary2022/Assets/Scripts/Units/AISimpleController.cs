using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISimpleController : Unit
{
    StateMachine stateMachine = new StateMachine();
    public Animator anim;

    Dictionary<AIStates, IState> state = new Dictionary<AIStates, IState>();

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
