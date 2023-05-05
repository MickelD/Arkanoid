using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Space(3), Header("Ball"), Space(1)]
    [SerializeField] private GameObject _ballPrefab;
    public float BallSpawnTime;
    [SerializeField] private Vector3 _ballStartPosition;
    [SerializeField] private int _startingLifes;

    private int _bricksToBreak;
    private int _lifes;
    public int Lifes
    {
        get 
        { 
            return _lifes; 
        }
        set 
        {
            _lifes = value;

            if (value <= 0)
            {
                PlayerDeath();
            }
            else
            {
                OnUpdateLifes?.Invoke(value);
            }
        }
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;

            OnScoreChanged?.Invoke(value);
        }
    }

    private int _highScore;
    public int HighScore
    {
        get
        {
            return _highScore;
        }
        set
        {
            _highScore = value;
            OnNewHighScore?.Invoke(value);
        }
    }

    public static System.Action<int> OnUpdateLifes;
    public static System.Action<float> OnStartBall;
    public static System.Action OnPlayerDied;
    public static System.Action<int> OnScoreChanged;
    public static System.Action<int> OnNewHighScore;
    public static System.Action OnLevelCleared;

    private void Start()
    {
        HighScore = 0;
    }

    //start does not call when traslading objects between scenes, Level Manager, a non-persistent object, is the one that resets some of the GM's values
    public void StartGame()
    {
        Lifes = _startingLifes;
        OnNewHighScore.Invoke(HighScore);

        _bricksToBreak = FindObjectsOfType<Brick>().Length;

        StartBall();
    }
    public void RestartGame()
    {
        Score = 0;
        SceneManager.LoadScene(0);
    }

    public void TryLoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LostBall()
    {
        Lifes--;
        if (Lifes > 0) 
        {
            StartBall();
        } 
    }

    public void StartBall()
    {
        OnStartBall?.Invoke(BallSpawnTime);
        Instantiate(_ballPrefab, _ballStartPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_ballStartPosition, 0.25f);
    }

    private void PlayerDeath()
    {
        if(Score > HighScore)
        {
            HighScore = Score;
        }

        OnPlayerDied?.Invoke();
    }

    public void BrickBroken()
    {
        _bricksToBreak--;

        if (_bricksToBreak <= 0)
        {
            if (Score > HighScore)
            {
                HighScore = Score;
            }

            OnLevelCleared?.Invoke();
        }
    }
}
