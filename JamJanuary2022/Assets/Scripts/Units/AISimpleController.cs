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
        IDLE,
        PATROL,
        CHASEAIR,
        REST,
        PROXIMITY,
        SHOOT,
        DIE
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();

        state[AIStates.PROXIMITY] = new IProximity(this);
        state[AIStates.DIE] = new IDie(this);
        state[AIStates.SHOOT] = new IShoot(this);
        state[AIStates.CHASEAIR] = new IChaseAir(this);
        state[AIStates.REST] = new IRest(this);

        stateMachine.ChangeState(state[AIStates.CHASEAIR]);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position - new Vector3(0, -1, 0), Vector3.down);

        if (Physics.Raycast(downRay, out hit, 50f))
        {
            if(hit.distance > 0.1f)
            {
                character.Move(new Vector3(0, -9.82f, 0) * Time.deltaTime);
            }
        }


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
