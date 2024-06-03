using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeScript : MonoBehaviour
{
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPosition = (transform.localPosition);
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude * Time.deltaTime;
            float y = Random.Range(-1f, 1f) * magnitude * Time.deltaTime;
            transform.localPosition = new Vector3(x, y, originalPosition.z);
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originalPosition;
    }
}
