using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Space(3), Header("Variables"), Space(1)]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;

    [Space(3), Header("Components"), Space(1)]
    [SerializeField] private Rigidbody _rb;

    private void FixedUpdate()
    {
        _rb.velocity = Vector3.Lerp(_rb.velocity, _maxSpeed * InputManager.Instance.xInput * transform.right, Time.fixedDeltaTime * _acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerObject triggerObject = other.GetComponent<TriggerObject>();

        if (triggerObject != null)
        {
            triggerObject.OnPickUpItem(this);
        }
    }
}
