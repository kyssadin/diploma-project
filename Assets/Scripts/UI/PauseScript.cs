using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Pause UI")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject panel;
    [Header("Keys")]
    [SerializeField] KeyCode pauseKey;
    [Header("End UI")]
    [SerializeField] GameObject endUI;
    [SerializeField] TextMeshProUGUI bestResults;
    [SerializeField] TextMeshProUGUI currentResult;
    private bool pause = false;
    private bool end = false;

    // Update is called once per frame
    void Update()
    {
        if (end) { return; }
        if (Input.GetKeyDown(pauseKey))
        {
            if (!pause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseUI.SetActive(true);
        panel.SetActive(true);
        AudioListener.pause = true;
        pause = true;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.SetActive(false);
        panel.SetActive(false);
        AudioListener.pause = false;
        pause = false;
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("Main Menu");
    }
    public void Restart()
    {
        bool isLevel2 = GameObject.FindGameObjectWithTag("Difficulty Manager").GetComponent<DifficultyManager>().level2;
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (isLevel2)
        {
            SceneManager.LoadScene("Level2");
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }

    public void Finished(ResultsDataHolder results, ResultsList resultList)
    {
        end = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        panel.SetActive(true);
        endUI.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0;
        for (int i = 0; i < resultList.values.Length; i++)
        {
            if (resultList.values[i] == null)
            {
                break;
            }
            bestResults.text += $"{i+1}. Очки: {resultList.values[i].score:F3} Час: {resultList.values[i].timeElapsed:F3}\n";
        }
        currentResult.text = $"Очки: {results.score:F3} Час: {results.timeElapsed:F3}";

    }
}
