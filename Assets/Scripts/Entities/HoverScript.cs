using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScript : MonoBehaviour
{
    public MeshRenderer rend;
    public Color hoverColor = Color.green;
    public Color defaultColor = Color.white;
    public Color clickColor = Color.red;
    private Color endColor;
    private bool coroutineActive = false;
    TargetScript targetScript;
    LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
        targetScript = GetComponent<TargetScript>();
        rend.material.color = defaultColor;
    }

    private void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

            if (!coroutineActive) 
        {
            rend.material.color = hoverColor;
        }
        else { endColor = hoverColor; }

    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !levelManager.WaveStarted)
        {
            return;
        }
        if (!coroutineActive)
        {
            bool choice = targetScript.correctChoice;
            rend.material.color = clickColor;
            Debug.Log("Clicked on the cube");
            targetScript.correctChoice = false;
            levelManager.ClearedWave(choice);
            coroutineActive = true;
            if (levelManager.isLevel2)
            {
                RevertColor();
            }
            else
            {
                StartCoroutine(RevertDelay(0.5f));
            }
        }
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!coroutineActive)
        {
            rend.material.color = defaultColor;
            
        }
        endColor = defaultColor;
    }

    IEnumerator RevertDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Color startColor = rend.material.color;
        
        float elapsedTime = 0f;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / delay);
            rend.material.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        RevertColor();
    }

    void RevertColor()
    {
        rend.material.color = defaultColor;
        coroutineActive = false;
    }

}
