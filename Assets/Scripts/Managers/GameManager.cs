using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Yarn.Unity;

public enum GameState
{
    MainMenu,
    Prepare,
    Interrogation
}

public enum PlayerState
{
    Talk,
    Review,
    ChooseEvidence,
    Inspecting
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
    public YarnCommandManager YarnCommandManager { get; private set; }

    public static event Action Advance;
    public static event Action StartInterrogation;
    public static event Action StartPrepare;
    public static event Action GoToMainMenu;
    public static event Action<PlayerState> ChangePlayerState;

    public override void Awake()
    {
        base.Awake();
        if (SceneManager.GetActiveScene().name == "Interrogation" && SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("Documents", LoadSceneMode.Additive);
        }
    }

    private void Start()
    {
#if !UNITY_EDITOR
        string currentCheckpoint = PlayerPrefs.GetString("Checkpoint", "Start");
        if (YarnCommandManager.Runner.IsDialogueRunning)
        {
            YarnCommandManager.Runner.Stop();
            YarnCommandManager.Runner.StartDialogue(currentCheckpoint);
        }
        else
        {
            YarnCommandManager.Runner.startNode = currentCheckpoint;
        }
#endif
    }

    private void Update()
    {
        //TODO: Make button?
        if (PlayerState == PlayerState.Review || PlayerState == PlayerState.ChooseEvidence)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                YarnCommandManager.Runner.Stop();
                PlayerState = PlayerState.Talk;
            }
        } else if (PlayerState == PlayerState.Talk)
        {
            
        }
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

    private string currentTalkNode;
    private bool escPressed;
    private void SetPlayerState(PlayerState value)
    {
        switch (value)
        {
            // case (PlayerState.ChooseEvidence):
            // case (PlayerState.Inspecting):
            case (PlayerState.Review):
            {
                if (playerState == PlayerState.Talk)
                {
                    currentTalkNode = YarnCommandManager.Runner.CurrentNodeName;
                }
                break;
            }
            case (PlayerState.Talk):
            {
                if (playerState == PlayerState.Review)// || playerState == PlayerState.ChooseEvidence || playerState == PlayerState.Inspecting)
                {
                    YarnCommandManager.Runner.Stop();
                    YarnCommandManager.Runner.StartDialogue(currentTalkNode);
                }
                break;
            }
        }
        ChangePlayerState?.Invoke(value);
        playerState = value;
    }

    public static void TriggerAdvance()
    {
        Advance?.Invoke();
    }

    public void SetYarnCommandManager(YarnCommandManager ycm)
    {
        YarnCommandManager = ycm;
    }

    public void SetCurrentNode(string node)
    {
        currentTalkNode = node;
    }
}
