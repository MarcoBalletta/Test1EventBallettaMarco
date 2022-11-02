using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private UIManager Ui;
    [SerializeField] private Board Board;
    private Board boardRef;

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
    public delegate void MoveFinished();
    public MoveFinished moveFinished;
    public delegate void CombinationMade(Colors typeColor);
    public CombinationMade combinationMade;

    public StateManager StateManager { get => stateManager; }
    public float PreGameTimer { get => preGameTimer; }
    public float GameTimer { get => gameTimer; }
    public int Rows { get => rows; }
    public int Columns { get => columns; }
    public int Points { get => points; }
    public int HighScore { get => highScore; }

    private void Awake()
    {
        stateManager = GetComponent<StateManager>();
        boardRef = Instantiate(Board, Vector3.zero, Quaternion.identity);
        boardRef.Init(this);
    }

    private void Start()
    {
        preGameStart += PreGameEnd;
        preGameStart();
        gameStart += GameEnd;
        combinationMade += SumPoints;
        reset += ResetCalled;
        highScore = PlayerPrefs.GetInt("HighScore" + SceneManager.GetActiveScene().name, 0);
    }

    private void PreGameEnd()
    {
        StartCoroutine(StateEndTimer("state:game", preGameTimer));
    }

    private IEnumerator StateEndTimer(string id, float timer)
    {
        yield return new WaitForSeconds(timer + Time.deltaTime);
        stateManager.ChangeState(id);
    }

    private void GameEnd()
    {
        StartCoroutine(StateEndTimer("state:endgame", gameTimer));
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