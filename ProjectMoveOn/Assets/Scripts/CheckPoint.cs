using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> checkPoint;

    private Vector3 _spawnPoint;
    private void Start()
    {
        _spawnPoint = gameObject.transform.position;
    }

    private void Update()
    {
        if (player.transform.position.y < -20f)
        {
            player.transform.position = _spawnPoint;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            _spawnPoint = gameObject.transform.position;
            Destroy(other.gameObject);
        }
    }
}
