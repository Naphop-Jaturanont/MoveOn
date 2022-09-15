using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData _gameData;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }
    
    public void LoadGame()
    {
        // TODO - Load any saved data from a file using the data handler
        // if no data can be Loaded, initialize to a new game
        if (this._gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        // TODO - push the loaded data to all other scripts that need it
    }
    
    public void SaveGame()
    {
        // TODO - pass the data to other scripts so they can update it
        
        // TODO - save that data to a file using the data handler
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
