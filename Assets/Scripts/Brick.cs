using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] private int _maxHP;
    private Material _brickMaterial;

    [SerializeField] private GameObject _destructionParticles;
    [SerializeField] private float _particleLifetime;
    private Vector3Int _pointsPerHealth;
    private Vector2 _pointsValueTimeThresholds;

    [SerializeField] private AudioCue _bounceSound;

    private int _hp;
    public int HP
    {
        get
        {
            return _hp;
        }

        set
        {
            _hp = value;

            if(_hp <= 0)
            {
                Break();
            }
            else
            {
                //Update Material
                _brickMaterial.SetFloat("_CrackDensity", Mathf.Lerp(1f, 0f, (float)_hp / _maxHP));
                _brickMaterial.SetFloat("_CrackStrenght", Mathf.Lerp(1f, 0f, (float)_hp / _maxHP));
            }
        }
    }

    private void OnEnable()
    {
        LevelManager.OnDefineBrickScoringRules += SetScoringRules;
    }

    private void OnDisable()
    {
        LevelManager.OnDefineBrickScoringRules -= SetScoringRules;
    }

    private void Start()
    {
        _brickMaterial = _meshRenderer.material;

        HP = _maxHP;
    }

    public void DealDamage()
    {
        HP--;
    }

    private void Break()
    {
        if (Time.timeSinceLevelLoad <= _pointsValueTimeThresholds.x)
        {
            GameManager.Instance.Score += _pointsPerHealth.x;
        }
        else if (Time.timeSinceLevelLoad <= _pointsValueTimeThresholds.y)
        {
            GameManager.Instance.Score += _pointsPerHealth.y;
        }
        else
        {
            GameManager.Instance.Score += _pointsPerHealth.z;
        }

        Destroy(gameObject);
        Destroy(Instantiate(_destructionParticles, transform.position, Quaternion.identity), _particleLifetime);

        GameManager.Instance.BrickBroken();
    }

    private void SetScoringRules(Vector3Int tierValues, Vector2 timeThresholds)
    {
        _pointsPerHealth = tierValues;
        _pointsValueTimeThresholds = timeThresholds;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlayCueAtPoint(_bounceSound, transform.position);
    }
}
