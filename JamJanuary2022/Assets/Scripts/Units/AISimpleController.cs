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

    [SerializeField] GameObject sfxPop;
    [SerializeField] GameObject vfxPop;

    public enum AIStates
    {
        CHASEAIR,
        REST,
        HOVER,
        BOMB,
        FLEE,
        DIE
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state[AIStates.DIE] = new IDie(this);
        state[AIStates.CHASEAIR] = new IChase(this);
        state[AIStates.REST] = new IRest(this);
        state[AIStates.HOVER] = new IHover(this);
        state[AIStates.BOMB] = new IBomb(this);
        state[AIStates.FLEE] = new IFlee(this);

        float value = Random.value;

        if(value > 0.3f)
        {
            stateMachine.ChangeState(state[AIStates.REST]);
        }
        else
        {
            stateMachine.ChangeState(state[AIStates.CHASEAIR]);
        }
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
            AlertAI();

            FindObjectOfType<MapGenerator>().OnKill(transform.position);
            //stateMachine.ChangeState(state[AIStates.DIE]);
            GameObject.Instantiate(sfxPop, transform.position, Quaternion.identity);
            GameObject.Instantiate(vfxPop, transform.position, Quaternion.identity);
            GameObject.FindObjectOfType<ScoreSystem>().Add(1);
            DestroyUnit();
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

    public void AlertAI()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 12f);

        foreach(Collider collider in colliders)
        {
            if(collider.gameObject.tag == "Enemy")
            {
                AISimpleController tempController = collider.GetComponent<AISimpleController>();

                if (tempController.stateMachine.currentState == state[AIStates.REST])
                {
                    tempController.stateMachine.ChangeState(tempController.state[AIStates.FLEE]);
                }
            }
        }
    }
}
