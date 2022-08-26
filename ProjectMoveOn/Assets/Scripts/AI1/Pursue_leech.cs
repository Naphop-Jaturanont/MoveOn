using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pursue_leech : State_leech
{
    //private GameObject[] waypoints => WPManager.Instance.getWPPosition();
    int currentIndex = 0;
    public Pursue_leech(GameObject npc, NavMeshAgent agent,Transform player, TextMesh txtStatus) :base(npc,agent,player,txtStatus)
    {
        name = StateStatus.Pursue;
        agent.speed = 2;
        agent.isStopped = false;
        agent.stoppingDistance = 0;
        agent.ResetPath();
    }

    public override void Enter()
    {
        txtStatus.text = "Patrol";

        /*float lastDist = Mathf.Infinity;
        currentIndex = 0;
        for(int i = 0; i < waypoints.Length; i++)
        {
            GameObject thisWp = waypoints[i];
            float distance = Vector3.Distance(thisWp.transform.position, npc.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }*/
        base.Enter();
    }

    public override void Update()
    {
        /*if(agent.remainingDistance < 1)
        {
            if(currentIndex>=waypoints.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(waypoints[currentIndex].transform.position);

        }*/
        if (DistancePlayer() < 10)//add in state
        {
            nextState = new Attack_leech (npc, agent, player, txtStatus);
            stage = EventState.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
