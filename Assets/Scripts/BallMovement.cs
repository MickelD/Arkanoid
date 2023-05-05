using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Space(3), Header("Speed"), Space(1)]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _startSpeedFraction;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _bounceTempSpeedMultiplier;
    [SerializeField] private float _bounceMaxSpeedIncrease;
    [SerializeField] private float _maxSpeed;
    private float _speed;

    [Space(3), Header("Trajectory correction"), Space(1)]
    [SerializeField] private Vector2 _minimumTolerableVelocity;
    [SerializeField] private Vector2 _correctionStep;

    [Space(3), Header("Collision Fixes"), Space(1)]
    [SerializeField] private float _minSpeedToTriggerReroute;
    [SerializeField] private float _maxTimeInRest;
    [SerializeField] private float _correctionStepDegrees;

    [Space(3), Header("Visuals"), Space(1)]
    [SerializeField] private ParticleSystem _ballParticles;
    [SerializeField] private Vector2Int _bounceParticleEmissionCount;
    [SerializeField] private GameObject _pulsePrefab;

    private void OnEnable()
    {
        //I would pretty much love to use anonymous functions for such a simple delegate, but those are impossible to unsubscribe as far as I know so i dont get to play smart
        GameManager.OnLevelCleared += DestroyBall;
    }

    private void OnDestroy()
    {
        GameManager.OnLevelCleared -= DestroyBall;
    }

    private IEnumerator Start()
    {
        float maxSpeed = _maxSpeed;
        _speed = 0f;
        _maxSpeed = 0f;

        yield return new WaitForSeconds(GameManager.Instance.BallSpawnTime);

        Instantiate(_pulsePrefab, transform.position, Quaternion.identity);

        StartCoroutine(CollisionFixes());

        _maxSpeed = maxSpeed;
        transform.right = Random.insideUnitCircle;
        _speed = _maxSpeed * _startSpeedFraction;
    }

    private void FixedUpdate()
    {
        _speed = Mathf.Lerp(_speed, _maxSpeed, _acceleration * Time.fixedDeltaTime);

        _rb.velocity = transform.right * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //direction Modifications
        Vector3 _transformRight = transform.right;

        _transformRight = (Vector3.Reflect(_transformRight, collision.GetContact(0).normal)).normalized;

        //trajectory correction (avoids infinite parallel bouncing)
        _transformRight.y = (_transformRight.y <= _minimumTolerableVelocity.y) ? _transformRight.y + _correctionStep.y * Mathf.Sign(_transformRight.y) : _transformRight.y;
        _transformRight.x = (_transformRight.x <= _minimumTolerableVelocity.x) ? _transformRight.x + _correctionStep.x * Mathf.Sign(_transformRight.x) : _transformRight.x;

        //set direction
        transform.right = _transformRight;

        //speed modifications
        _maxSpeed += _bounceMaxSpeedIncrease;
        _speed = _maxSpeed * _bounceTempSpeedMultiplier;

        //visuals
        _ballParticles.Emit(Random.Range(_bounceParticleEmissionCount.x, _bounceParticleEmissionCount.y + 1));

        //handle possible brick collision
        Brick brick = collision.gameObject.GetComponent<Brick>();
        if (brick != null)
        {
            brick.DealDamage();
        }
    }

    private IEnumerator CollisionFixes()
    {
        while (gameObject.activeInHierarchy)
        {
            float speed = _rb.velocity.magnitude;

            yield return new WaitForSeconds(_maxTimeInRest);

            if (speed <= _minSpeedToTriggerReroute && _rb.velocity.magnitude <= _minSpeedToTriggerReroute)
            {
                transform.Rotate(new Vector3(0f, 0f, _correctionStepDegrees));
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.LostBall();
        DestroyBall();
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}
