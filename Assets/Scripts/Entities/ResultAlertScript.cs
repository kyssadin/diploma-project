using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultAlertScript : MonoBehaviour
{
    [SerializeField] List<AudioClip> clipList;
    [SerializeField] AudioSource audioSource;

    public void PlayResult(bool correct)
    {
        if (correct)
        {
            audioSource.PlayOneShot(clipList[0]);
        }
        else
        {
            audioSource.PlayOneShot(clipList[1]);
        }
    }

    public void PlayCountdown(bool end)
    {
        if (end)
        {
            audioSource.PlayOneShot(clipList[3]);
        }
        else
        {
            audioSource.PlayOneShot(clipList[2]);
        }
    }
}
