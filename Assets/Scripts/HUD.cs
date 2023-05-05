using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Space(3), Header("Menus"), Space(1)]
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _levelClearedMenu;

    [Space(3), Header("Lifes Counter"), Space(1)]
    [SerializeField] private GameObject _lifesCounter;

    [Space(3), Header("Score"), Space(1)]
    [SerializeField] private string _scoreDisplayFormat;
    [SerializeField] private TextMeshProUGUI _scoreCounter;
    [SerializeField] private TextMeshProUGUI _finalScoreCounter;
    [SerializeField] private TextMeshProUGUI _bestScoreCounter;
    [SerializeField] private TextMeshProUGUI _finalHighScoreText;
    private int _bestScore;

    [Space(3), Header("Countdown"), Space(1)]
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private Animator _countDownAnimator;

    [Space(3), Header("Flash"), Space(1)]
    [SerializeField] private Animator _flashAnimator;

    private void OnEnable()
    {
        GameManager.OnUpdateLifes += UpdateLifesCounter;
        GameManager.OnStartBall += StartBallCounterCoroutine;
        GameManager.OnPlayerDied += GameOverMenuUI;
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnNewHighScore += UpdateBestScore;
        GameManager.OnLevelCleared += LevelClearedMenuUI;
    }

    private void OnDisable()
    {
        GameManager.OnUpdateLifes -= UpdateLifesCounter;
        GameManager.OnStartBall -= StartBallCounterCoroutine;
        GameManager.OnPlayerDied -= GameOverMenuUI;
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnNewHighScore -= UpdateBestScore;
        GameManager.OnLevelCleared -= LevelClearedMenuUI;
    }

    private void Start()
    {
        _hud.SetActive(true);
        _finalHighScoreText.gameObject.SetActive(false);
        _gameOverMenu.SetActive(false);
        _levelClearedMenu.SetActive(false);
    }

    private void UpdateLifesCounter(int l)
    {
        for (int i = l; i < _lifesCounter.transform.childCount; i++)
        {
            _lifesCounter.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void StartBallCounterCoroutine(float seconds)
    {
        _flashAnimator.SetTrigger("Flash");
        StartCoroutine(BallSpawnCountdown(seconds));
    }

    private void GameOverMenuUI()
    {
        _flashAnimator.SetTrigger("Flash");

        _finalHighScoreText.gameObject.SetActive(true);
        _hud.SetActive(false);
        _levelClearedMenu.SetActive(false);

        _gameOverMenu.SetActive(true);
    }

    private void LevelClearedMenuUI()
    {
        _flashAnimator.SetTrigger("Flash");

        _finalHighScoreText.gameObject.SetActive(true);
        _hud.SetActive(false);
        _gameOverMenu.SetActive(false);

        _levelClearedMenu.SetActive(true);
    }

    public void Retry()
    {
        GameManager.Instance.RestartGame();
    }

    public void NextLevel()
    {
        GameManager.Instance.TryLoadNextLevel();
    }

    private IEnumerator BallSpawnCountdown(float seconds)
    {
        _countDownText.gameObject.SetActive(true);
        for (int i = (int)seconds; i > 0; i--)
        {
            _countDownText.gameObject.SetActive(true);

            _countDownText.text = i.ToString();
            _countDownAnimator.SetTrigger("Appear");

            yield return new WaitForSeconds(1f);

            _countDownText.gameObject.SetActive(false);
        }
    }

    private void UpdateScore(int score)
    {
        _finalHighScoreText.text = score > _bestScore ? "New Best!" : "Final Score";

        _scoreCounter.text = _finalScoreCounter.text = score.ToString(_scoreDisplayFormat);
    }

    private void UpdateBestScore(int highScore)
    {
        _bestScore = highScore;
        _bestScoreCounter.text = "Best " + highScore.ToString(_scoreDisplayFormat);
        Color col = _bestScoreCounter.color;
        col.a = (highScore != 0).GetHashCode();
        _bestScoreCounter.color = col;
    }
}
