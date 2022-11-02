using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : BaseGameState
{
    public override void Enter(GameManager gm)
    {
        stateName = "Game";
        gameManager = gm;
        gameManager.gameStart();
    }

    public override void Exit()
    {

    }
}
