using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameState
{
    MainMenu,
    Prepare,
    Interrogation
}

public enum PlayerState
{
    Talk,
    Review
}

public class GameManager : Singleton<GameManager>
{
    private GameState _gamestate;
    public GameState GameState //GameState cannot be set without calling SetGameState
    {
        set { SetGameState(value); }
        get { return _gamestate; }
    }

    [SerializeField] private PlayerState playerState = PlayerState.Talk;
    public PlayerState PlayerState 
    {
        set { SetPlayerState(value); }
        get { return playerState; }
    }

    public static event Action Advance;
    public static event Action StartInterrogation;
    public static event Action StartPrepare;
    public static event Action GoToMainMenu;
    public static event Action<PlayerState> ChangePlayerState;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetGameState(GameState newGameState)
    {
        // Debug.Log("");
        switch (newGameState)
        {
            case (GameState.MainMenu):
            {
                GoToMainMenu?.Invoke();
                break;
            }
            case (GameState.Prepare):
            {
                StartPrepare?.Invoke();
                break;
            }
            case (GameState.Interrogation):
            {
                StartInterrogation?.Invoke();
                break;
            }
        }

        _gamestate = newGameState;
    }
    
    private void SetPlayerState(PlayerState value)
    {
        playerState = value;
        ChangePlayerState?.Invoke(playerState);
    }

    public static void TriggerAdvance()
    {
        Advance?.Invoke();
    }
}
