using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameState : BaseGameState
{
    public override void Enter(GameManager gm)
    {
        stateName = "PreGame";
        gameManager = gm;
        gameManager.preGameStart();
    }

    public override void Exit()
    {
        
    }
}
