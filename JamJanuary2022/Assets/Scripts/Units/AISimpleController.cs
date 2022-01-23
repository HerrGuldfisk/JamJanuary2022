using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISimpleController : Unit
{
    public StateMachine stateMachine = new StateMachine();
    public Weapon weapon;
    public Animator anim;
    public CharacterController character;

    public bool onGround = false;

    public Dictionary<AIStates, IState> state = new Dictionary<AIStates, IState>();

    public enum AIStates
    {
        CHASEAIR,
        REST,
        HOVER,
        BOMB,
        DIE
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();

        state[AIStates.DIE] = new IDie(this);
        state[AIStates.CHASEAIR] = new IChase(this);
        state[AIStates.REST] = new IRest(this);
        state[AIStates.HOVER] = new IHover(this);
        state[AIStates.BOMB] = new IBomb(this);

        stateMachine.ChangeState(state[AIStates.REST]);
    }

    // Update is called once per frame
    void Update()
    {
        if(stateMachine.currentState != null)
        {
            // Debug.Log(stateMachine.currentState);
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

        if(collision.gameObject.tag == "Terrain")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            onGround = false;
        }
    }

    public void HearNoise()
    {

    }
}
