using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public BaseGameState CurrentState;
    public Dictionary<string, BaseGameState> statesList = new Dictionary<string, BaseGameState>();
    private GameManager gameManager;

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
        CurrentState?.Exit();
        CurrentState = statesList[id];
        CurrentState?.Enter(gameManager);
    }
}
