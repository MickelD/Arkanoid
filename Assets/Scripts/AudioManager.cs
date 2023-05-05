using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    /*
        This script works by having a bunch of children each with an Audio source.
        "All of them are deactivated and then the method PlayCueAtPoint takes the first usable audio source
        "and sets it aside for it to play its corresponding cue. When the sound is done, the chosen audio source
        "is disabled and returned to the list of candidates.
        "This system can play as many simultaneous audioclips as children this game object has
        "Important, all children must have an audiosource component"*/

    private List<AudioSource> _usableAudioSources;

    private void Start()
    {
        _usableAudioSources = new List<AudioSource>();

        foreach (Transform child in transform)
        {
            _usableAudioSources.Add(child.GetComponent<AudioSource>());
            child.gameObject.SetActive(false);
        }
    }

    public AudioSource PlayCueAtPoint(AudioCue audioCue, Vector3 location)
    {
        //Pooling Logic
        AudioSource selectedAudioSource = _usableAudioSources[0];
        //if this array is empty, that means we ran out of usable sources, and sould instead Instantiate a new One


        _usableAudioSources.Remove(selectedAudioSource);

        //set source values
        selectedAudioSource.clip = audioCue.SfxClip;
        selectedAudioSource.loop = audioCue.Loop;

        selectedAudioSource.volume = audioCue.Volume;
        selectedAudioSource.pitch = audioCue.Pitch;

        selectedAudioSource.spatialBlend = audioCue.SpatialBlend;

        //play source at location
        selectedAudioSource.transform.position = location;

        selectedAudioSource.gameObject.SetActive(true);
        selectedAudioSource.Play();

        if (!audioCue.Loop)
        {
            StartCoroutine(PlayingAudio(selectedAudioSource, audioCue.SfxClip.length));
        }

        return selectedAudioSource;
    }

    private IEnumerator PlayingAudio(AudioSource audioSource, float duration)
    {
        yield return new WaitForSeconds(duration);

        //RETURN TO ORIGINAL LOCATION

        _usableAudioSources.Add(audioSource);
        audioSource.gameObject.SetActive(false);
    }
}
