using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Review,
    Interrogation
}

public class GameManager : Singleton<GameManager>
{
    private GameState _gamestate;
    public GameState GameState //GameState cannot be set without calling SetGameState
    {
        set { SetGameState(value); }
        get { return _gamestate; }
    }
    
    
    public static event Action Advance;
    public static event Action StartInterrogation;
    public static event Action StartReview;
    public static event Action GoToMainMenu;
    
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
            case (GameState.Review):
            {
                StartReview?.Invoke();
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

    public static void TriggerAdvance()
    {
        Advance?.Invoke();
    }
}
