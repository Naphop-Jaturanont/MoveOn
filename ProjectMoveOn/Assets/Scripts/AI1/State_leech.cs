using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_leech
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
    protected State_leech nextState;
    protected NavMeshAgent agent;
    protected TextMesh txtStatus;

    public State_leech(GameObject npc, NavMeshAgent agent, Transform player, TextMesh txtStatus)
    {
        this.npc = npc;
        this.agent = agent;
        this.stage = EventState.Enter;
        this.player = player;
        this.txtStatus = txtStatus;
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

    public State_leech Process()
    {
        State_leech result = this;
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
