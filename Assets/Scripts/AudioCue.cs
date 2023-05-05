using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioCue
{
    public AudioClip SfxClip;

    [Space(3), Header("Properties")]
    public float Volume;
    public float Pitch;
    public bool Loop;

    [Space(3), Header("Spatialization")]
    [Range(0f, 1f)] public float SpatialBlend;
}
