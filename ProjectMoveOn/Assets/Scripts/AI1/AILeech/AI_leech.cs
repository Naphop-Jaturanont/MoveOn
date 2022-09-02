using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AI_leech : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    public Animator animator;
    State_leech currentState;
    TextMeshProUGUI txtStatus;
    //Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        txtStatus = this.GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        currentState = new Idle_leech(this.gameObject, agent, player, txtStatus, animator);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
        //anim.SetInteger("Walk", 1);
    }
}