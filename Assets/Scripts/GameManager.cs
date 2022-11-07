using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Board Board;

    [Header ("GridStats")]
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    [Header ("Points")]
    [SerializeField] private int points;
    [SerializeField] private int highScore;

    [Header ("Timers")]
    [SerializeField] private float preGameTimer;
    [SerializeField] private float gameTimer;

    private StateManager stateManager;
    public delegate void PreGameStart();
    public PreGameStart preGameStart;
    public delegate void GameStart();
    public GameStart gameStart;    
    public delegate void EndGameStart();
    public EndGameStart endGameStart;
    public delegate void Reset();
    public Reset reset;
    public delegate void CombinationMade(Colors typeColor);
    public CombinationMade combinationMade;

    public float PreGameTimer { get => preGameTimer; }
    public float GameTimer { get => gameTimer; }
    public int Rows { get => rows; }
    public int Columns { get => columns; }
    public int Points { get => points; }
    public int HighScore { get => highScore; }

    private void Awake()
    {
        stateManager = GetComponent<StateManager>();
        Board boardRef = Instantiate(Board, Vector3.zero, Quaternion.identity);
        boardRef.Init(this);
    }

    private void Start()
    {
        //preGameStart();
        stateManager.ChangeState(Constants.STATE_ID_PREGAME);
        combinationMade += SumPoints;
        reset += ResetCalled;
        highScore = PlayerPrefs.GetInt("HighScore" + SceneManager.GetActiveScene().name, 0);
    }

    public void PreGameEnd()
    {
        stateManager.ChangeState(Constants.STATE_ID_GAME);
    }

    public void GameEnd()
    {
        stateManager.ChangeState(Constants.STATE_ID_ENDGAME);
    }

    private void SumPoints(Colors colorType)
    {
        switch (colorType)
        {
            case Colors.red:
                points += Constants.POINTS_RED;
                break;
            case Colors.blue:
                points += Constants.POINTS_BLUE;
                break;
            case Colors.green:
                points += Constants.POINTS_GREEN;
                break;
            case Colors.yellow:
                points += Constants.POINTS_YELLOW;
                break;
        }
    }

    public void ResetGame()
    {
        reset();
    }

    private void ResetCalled()
    {
        StopAllCoroutines();
        points = 0;
        stateManager.ChangeState(Constants.STATE_ID_PREGAME);
    }

    public bool CheckNewHighScore()
    {
        if(points > highScore)
        {
            PlayerPrefs.SetInt("HighScore" + SceneManager.GetActiveScene().name, points);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE_NAME);
    }
}
