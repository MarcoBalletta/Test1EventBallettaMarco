using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI preTimerText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI newHighScoreText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject panelEndGame;
    Coroutine coroutineTimer;
    private float timer;

    private void Awake()
    {
        gameManager.preGameStart += PreGameTimer;
        gameManager.gameStart += GameTimer;
        gameManager.endGameStart += EndGame;
        gameManager.reset += ResetGameUI;
    }

    public void PreGameTimer()
    {
        preTimerText.gameObject.SetActive(true);
        coroutineTimer = StartCoroutine(PreGameCountdown());
    }

    private IEnumerator PreGameCountdown()
    {
        preTimerText.text = gameManager.PreGameTimer.ToString();
        timer = gameManager.PreGameTimer;
        while(timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            preTimerText.text = timer.ToString();
        }
        preTimerText.gameObject.SetActive(false);
    }

    public void GameTimer()
    {
        timerText.gameObject.SetActive(true);
        StartCoroutine(GameCountdown());
    }

    private IEnumerator GameCountdown()
    {
        TimerText.text = gameManager.GameTimer.ToString();
        timer = gameManager.GameTimer;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            TimerText.text = timer.ToString();
        }
    }

    private void EndGame()
    {
        newHighScoreText.gameObject.SetActive(false);
        StopAllCoroutines();
        panelEndGame.SetActive(true);
        pointsText.text = gameManager.Points.ToString();
        if (gameManager.CheckNewHighScore())
        {
            highScoreText.text = pointsText.text;
            newHighScoreText.gameObject.SetActive(true);
        }
        else
        {
            highScoreText.text = gameManager.HighScore.ToString();
        }
    }

    private void ResetGameUI()
    {
        StopAllCoroutines();
        panelEndGame.SetActive(false);
        timerText.text = gameManager.GameTimer.ToString();
        preTimerText.text = gameManager.PreGameTimer.ToString();
        pointsText.text = "0";
    }

    public TextMeshProUGUI TimerText { get => timerText; set => timerText = value; }
}
