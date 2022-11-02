using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameState : BaseGameState
{
    public override void Enter(GameManager gm)
    {
        stateName = "EndGame";
        gameManager = gm;
        gameManager.endGameStart();
    }

    public override void Exit()
    {

    }
}
