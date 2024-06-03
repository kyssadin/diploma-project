using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [Header("Sound Cues")]
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] AudioSource source;
    public bool correctChoice = false;

    public void PlayCue()
    {
        correctChoice = true;
        source.pitch = Random.Range(0.8f, 1f);
        source.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
    }
}
