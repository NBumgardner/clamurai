using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const float RUN_SPEED = 10f;

    private StateMachine stateMachine = new StateMachine();
    private List<State> states = new List<State>();
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        states.Add(new StandState(this, stateMachine));
        states.Add(new RunState(this, stateMachine));
        stateMachine.Initialize(states[(int)PlayerStates.STAND]);
    }

    private void applyInputAndTransitionStates()
    {
        PlayerStates nextState;
        do
        {
            nextState = stateMachine.CurrentState.HandleInput();
            if (nextState != PlayerStates.NO_CHANGE)
            {
                stateMachine.ChangeState(states[(int)nextState]);
            }
        } while (nextState != PlayerStates.NO_CHANGE);
    }

    // Update is called once per frame
    void Update()
    {
        applyInputAndTransitionStates();
        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }
}
