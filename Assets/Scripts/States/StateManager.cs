using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private BaseGameState currentState;
    private Dictionary<string, BaseGameState> statesList = new Dictionary<string, BaseGameState>();
    private GameManager gameManager;

    public Dictionary<string, BaseGameState> StatesList { get => statesList; }
    public BaseGameState CurrentState { get => currentState; }

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        CreateDictionary();
    }

    private void CreateDictionary()
    {
        statesList.Add(Constants.STATE_ID_PREGAME, new PreGameState());
        statesList.Add(Constants.STATE_ID_GAME, new GameState());
        statesList.Add(Constants.STATE_ID_ENDGAME, new EndGameState());
    }

    public void ChangeState(string id)
    {
        currentState?.Exit();
        currentState = statesList[id];
        currentState?.Enter(gameManager);
    }
}
