using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Idle_leech : State_leech
{
    //private GameObject[] hides => HideManager.Instance.getHidePosition();
    public Idle_leech(GameObject npc, UnityEngine.AI.NavMeshAgent agent, Transform player, TextMeshProUGUI txtStatus,Animator animator) : base(npc, agent, player, txtStatus,animator)
    {
        name = StateStatus.Idle;
        //agent.speed = 12;
        agent.isStopped = false;
        //agent.ResetPath();

    }

    public override void Enter()
    {
        txtStatus.text = "Idle";
        base.Enter();
    }

    public override void Update()
    {
        //playanimation or patrol
        
        //Hide();
        if (DistancePlayer() < 10)
        {
            nextState = new Pursue_leech(npc, agent, player, txtStatus,animator);
            stage = EventState.Exit;
        }
    }

    /*public void Hide()
    {
        float farFactor = 20;
        float lastDist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < hides.Length; i++)
        {
            Vector3 hideDir = hides[i].transform.position - player.transform.position;
            Vector3 hidePos = hides[i].transform.position + hideDir.normalized * farFactor;

            float dist = Vector3.Distance(npc.transform.position, hidePos);
            if (dist < lastDist)
            {
                chosenSpot = hidePos;
                lastDist = dist;
            }
        }
        agent.SetDestination(chosenSpot);
    }*/
}
