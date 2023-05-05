using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Space(3), Header("Scoring"), Space(1)]
    [Tooltip("Bricks give less points the longer they take to be destroyed"), SerializeField] private Vector3Int _brickPointsPerHpTiers;
    [SerializeField] private Vector2 _timePerBrickScoreTier;

    public static System.Action<Vector3Int, Vector2> OnDefineBrickScoringRules;

    void Start()
    {
        OnDefineBrickScoringRules.Invoke(_brickPointsPerHpTiers, _timePerBrickScoreTier);
        GameManager.Instance.StartGame();
    }
}
