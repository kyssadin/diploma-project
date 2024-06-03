using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Toggles")]
    [SerializeField] Toggle difficulty1;
    [SerializeField] Toggle difficulty2;
    [SerializeField] Toggle difficulty3;
    [SerializeField] Slider waveSlider;
    
    
    private DifficultyManager difficultyManager;

    public void Start()
    {
        difficultyManager = GameObject.FindGameObjectWithTag("Difficulty Manager").GetComponent<DifficultyManager>();
        if (difficultyManager.Difficulty == 1)
        {
            difficulty1.isOn = true;
            difficulty2.isOn = false;
            difficulty3.isOn = false;
        }
        else if (difficultyManager.Difficulty == 1.5)
        {
            difficulty2.isOn = true;
            difficulty1.isOn = false;
            difficulty3.isOn = false;
        }
        else if(difficultyManager.Difficulty == 2)
        {
            difficulty3.isOn = true;
            difficulty2.isOn = false;
            difficulty1.isOn = false;
        }

        waveSlider.value = difficultyManager.Waves;
    }

    public void DifficultyChoice()
    {
        if (difficulty1.isOn)
        {
            difficultyManager.Difficulty = 1;
        }
        else if (difficulty2.isOn)
        {
            difficultyManager.Difficulty = 1.5f;
        }
        else if (difficulty3.isOn)
        {
            difficultyManager.Difficulty = 2;
        }

    }

    public void WavesNumberChoice()
    {
        difficultyManager.Waves = Convert.ToInt32(waveSlider.value);
    }



    public void StartLevel1()
    {
        SceneManager.LoadScene("Level1");
        difficultyManager.level2 = false;
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level2");
        difficultyManager.level2 = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
