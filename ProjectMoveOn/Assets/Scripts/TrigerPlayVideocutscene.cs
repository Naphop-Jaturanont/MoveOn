using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrigerPlayVideocutscene : MonoBehaviour
{
    public GameState where;
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            Gamemanager.Instance.UpdateGameState(where);
        }
    }
}
