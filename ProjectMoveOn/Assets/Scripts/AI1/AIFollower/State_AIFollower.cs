using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class State_AIFollower
{
    public enum StateStatus
    {
        Idle, Pursue, Died, Attack
    };

    public enum EventState
    {
        Enter, Update, Exit
    };

    public StateStatus name;
    protected EventState stage;
    protected GameObject npc;
    protected Transform player;
    protected State_AIFollower nextState;
    protected NavMeshAgent agent;
    protected TextMeshProUGUI txtStatus;
    protected Animator animator;

    public State_AIFollower(GameObject npc, NavMeshAgent agent, Transform player, TextMeshProUGUI txtStatus, Animator animator)
    {
        this.npc = npc;
        this.agent = agent;
        this.stage = EventState.Enter;
        this.player = player;
        this.txtStatus = txtStatus;
        this.animator = animator;
    }

    public virtual void Enter()
    {
        stage = EventState.Update;
    }

    public virtual void Update()
    {
        stage = EventState.Update;
    }

    public virtual void Exit()
    {
        stage = EventState.Exit;
    }

    public State_AIFollower Process()
    {
        State_AIFollower result = this;
        if (stage == EventState.Enter)
        {
            Enter();
        }

        else if (stage == EventState.Update)
        {
            Update();
        }

        if (stage == EventState.Exit)
        {
            Exit();
            result = nextState;
        }
        return result;
    }

    public float DistancePlayer()
    {
        return Vector3.Distance(npc.transform.position, player.position);
    }
}
