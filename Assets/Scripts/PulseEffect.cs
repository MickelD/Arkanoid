using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [SerializeField] private float _pulseSpeed;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private AnimationCurve _sizeOverLife;
    [SerializeField] private AnimationCurve _alphaOverLife;
    private float _time;
    private Material _material;

    void Start()
    {
        _material = _meshRenderer.material;
    }

    private void Update()
    {
        _time += _pulseSpeed * Time.deltaTime;
        _material.SetFloat("_SizeT", _sizeOverLife.Evaluate(_time));
        _material.SetFloat("_alphaT", _alphaOverLife.Evaluate(_time));

        if (_time >= 1)
        {
            Destroy(gameObject);
        }
    }
}
