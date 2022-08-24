using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum GameState
{
    VideoCutsceneStartPoint, VideoCutsceneFall, VideoCutscenePassTunnel, Gameplay,Gameover
}

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;
    public Transform stratPoint;
    public GameObject player;
    public GameObject maincamera;
    public GameObject videoCameraStartPoint;
    public GameObject videoCamerafall;
    public GameObject videoCameraPassTunnel;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        player.transform.position = stratPoint.position;
        //UpdateGameState(GameState.VideoCut);
        UpdateGameState(GameState.Gameplay);
    }
    public void UpdateGameState(GameState gameState)
    {
        state = gameState;

        switch (gameState)
        {
            case GameState.VideoCutsceneStartPoint:
                videoCameraStartPoint.SetActive(true);
                videoCamerafall.SetActive(false);
                videoCameraPassTunnel.SetActive(false);
                maincamera.SetActive(false);
                PlayVideoCutScene();
                break;
            case GameState.VideoCutsceneFall:
                videoCameraStartPoint.SetActive(false);
                videoCamerafall.SetActive(true);
                videoCameraPassTunnel.SetActive(false);
                maincamera.SetActive(false);
                PlayVideoCutScene();
                break;
            case GameState.VideoCutscenePassTunnel:
                videoCameraStartPoint.SetActive(false);
                videoCamerafall.SetActive(false);
                videoCameraPassTunnel.SetActive(true);
                maincamera.SetActive(false);
                PlayVideoCutScene();
                break;
            case GameState.Gameplay:
                videoCameraStartPoint.SetActive(false);
                videoCamerafall.SetActive(false);
                videoCameraPassTunnel.SetActive(false);
                maincamera.SetActive(true);
                break;
            case GameState.Gameover:
                break;
        }
        OnGameStateChanged?.Invoke(gameState);
    }

    public void PlayVideoCutScene()
    {
        //PlayVideoCutscene
        //stop
        StartCoroutine("OnChangeCamera");
    }

    IEnumerator OnChangeCamera()
    {
        yield return new WaitForSeconds(10.0f);

        UpdateGameState(GameState.Gameplay);
    }
}




