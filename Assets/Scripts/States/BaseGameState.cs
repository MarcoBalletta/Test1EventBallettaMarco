using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseGameState
{
    protected GameManager gameManager;
    public string stateName;
    public virtual void Enter(GameManager gm) { }
    public virtual void Exit() { }
}
