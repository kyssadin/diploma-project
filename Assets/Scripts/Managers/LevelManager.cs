using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class LevelManager : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] List<TargetScript> target;
    [SerializeField] List<GameObject> targetsObjects;
    [SerializeField] List<ExplosionTriggerScript> speakers;
    [SerializeField] ResultAlertScript alertScript;
    [Header("UI")]
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject textCountDown;
    [SerializeField] GameObject waveCounter;
    [SerializeField] PauseScript pauseScript;
    [Header("Controller")]
    [SerializeField] PlayerMovement keyInput;
    [SerializeField] MouseMovement mouseInput;
    [SerializeField] FootstepsScript footInput;

    public float difficulty = 0;
    private int WaveNumber = 3;
    private int waveCount = 0;
    public int correctGuesses = 0;
    private List<int> activeTargetsIndices;
    private const int Level2TargetsActive = 6;

    private float timeForWave = 5f;
    private bool intensificationActive = false;

    public float currentScore = 0;
    private float waveTimeElapsed = -3; 
    public float totalTimeElapsed = -3; 
    private float comboMultiplier = 1; 

    public bool WaveStarted { get; private set; } = false;
    public bool isLevel2 = false;

    void Awake()
    {
        DifficultyManager dManager = FindObjectOfType<DifficultyManager>();
        difficulty = dManager.Difficulty;
        WaveNumber = dManager.Waves;
        isLevel2 = dManager.level2;
        
        if (difficulty >= 1.5)
        {
            intensificationActive = true;
            StartCoroutine(PlayExplosion());
            if (difficulty >= 2)
            {
                foreach (var speaker in speakers)
                {
                    speaker.shakeActive = true;
                }
            }
        }

        if (isLevel2)
        {
            UpdateActiveTargets();
        }
        keyInput.enabled = false;
        mouseInput.enabled = false;
        footInput.enabled = false;
        crosshair.SetActive(false);
        panel.SetActive(true);
        textCountDown.SetActive(true);
        StartCoroutine(CountDown(3));
    }

    private IEnumerator CountDown (int delay)
    {
        for (int i = delay; i > 0; i--)
        {
            textCountDown.GetComponent<TextMeshProUGUI>().text = i.ToString();
            alertScript.PlayCountdown(false);
            yield return new WaitForSeconds(1);
        }
        alertScript.PlayCountdown(true);
        panel.SetActive(false);
        textCountDown.SetActive(false);
        crosshair.SetActive(true);
        keyInput.enabled = true;
        mouseInput.enabled = true;
        footInput.enabled = true;
        StartCoroutine(NextWave());
    }

    private IEnumerator NextWave()
    {
        WaveStarted = false;
        waveCounter.GetComponent<TextMeshProUGUI>().text = $"Спроба: {waveCount}/{WaveNumber}\nПравильно: {correctGuesses}\nПоточний результат: {currentScore:F3}";
        if (waveCount == WaveNumber)
        {
            Debug.Log("Waves ended");
            ResultsDataHolder scores = new ResultsDataHolder()
            {
                score = currentScore,
                timeElapsed = totalTimeElapsed,
            };
            ResultsList results = ResultYieldScript.UpdateData(ResultYieldScript.GetData(), scores);
            waveCounter.SetActive(false);
            pauseScript.Finished(scores, results);
            ResultYieldScript.SaveData(results);
        }
        else
        {
            waveTimeElapsed = -1;
            yield return new WaitForSeconds(1);
            WaveStarted = true;
            if (isLevel2)
            {
                int targetIndex = activeTargetsIndices[Random.Range(0, activeTargetsIndices.Count)];
                target[targetIndex].PlayCue();
            }
            else
            {
                target[Random.Range(0, target.Count)].PlayCue();
            }
        }
        // target[0].PlayCue();
        
    }

    private void UpdateActiveTargets()
    {
        activeTargetsIndices = new List<int>();
        for (int i = 0; i < Level2TargetsActive; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, target.Count);
            }
            while (activeTargetsIndices.Contains(index));
            activeTargetsIndices.Add(index);
        }
        for (int i = 0; i < target.Count; i++) 
        {
            if (activeTargetsIndices.Contains(i))
            {
                targetsObjects[i].SetActive(true);
            }
            else
            {
                targetsObjects[i].SetActive(false);
            }
        }
    }
    public void ClearedWave(bool choice)
    {
        correctGuesses = choice ? correctGuesses + 1 : correctGuesses;
        if (choice)
        {
            currentScore += (2.5f / waveTimeElapsed) * difficulty * comboMultiplier;
            if (comboMultiplier <= 2)
            {
                comboMultiplier += 0.2f;
            }
            timeForWave *= 0.95f;
        }
        else
        {
            comboMultiplier = 1;
        }
        alertScript.PlayResult(choice);
        waveCount++;
        foreach (var target in target) // Resetting the correct choices
        {
            target.correctChoice = false;
        }
        if (isLevel2)
        {
            UpdateActiveTargets();
        }
        StartCoroutine(NextWave());
    }

    // Update is called once per frame
    void Update()
    {
        totalTimeElapsed += Time.deltaTime;
        waveTimeElapsed += Time.deltaTime;
        if (intensificationActive)
        {
            if (waveTimeElapsed > timeForWave)
            {
                ClearedWave(false);
            }
        }
        GraphicRaycaster raycaster = panel.GetComponent<GraphicRaycaster>();
        if ( panel != null && panel.activeSelf)
        {
            if ( raycaster != null)
            {
                raycaster.enabled = false;
            }
        }
        else
        {
            if (raycaster != null)
            {
                raycaster.enabled = true;
            }
        }
    }

    IEnumerator PlayExplosion()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            speakers[Random.Range(0, speakers.Count)].TriggerExplosion();
        }
    }
}
