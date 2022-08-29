using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> checkPoint;
    [SerializeField] private Vector3 vertorPoint;
    [SerializeField] private int dead;// int or float

    private void Update()
    {
        if (player.transform.position.y < -dead)
        {
            player.transform.position = vertorPoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        vertorPoint = player.transform.position;
        Destroy(other.gameObject);
    }
}
