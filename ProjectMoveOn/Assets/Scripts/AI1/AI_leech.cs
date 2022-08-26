using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_leech : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    State_leech currentState;
    TextMesh txtStatus;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        txtStatus = this.GetComponentInChildren<TextMesh>();
        currentState = new Idle_leech(this.gameObject, agent, player, txtStatus);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
        anim.SetInteger("Walk", 1);
    }
}