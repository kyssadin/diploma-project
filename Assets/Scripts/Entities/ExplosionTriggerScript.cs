using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class ExplosionTriggerScript : MonoBehaviour
{
    public AudioSource explosionSound;
    public ScreenShakeScript screenShake;
    public bool shakeActive = false;

    public void TriggerExplosion()
    {
        explosionSound.Play();
        if (shakeActive)
        {
            StartCoroutine(screenShake.Shake(2f, 25f));
        }
    }
}
