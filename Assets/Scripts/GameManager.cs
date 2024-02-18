using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private float worldSpeed;

    [SerializeField]
    private GameObject obstacleEasy;

    [SerializeField]
    private float startingX;

    [SerializeField]
    private float spawnDelay;

    [SerializeField]
    private float leftLimit;

    [SerializeField]
    private float baseObstacleY;

    [SerializeField]
    private Transform birdTransform;

    [SerializeField]
    private Canvas deathMenu;

    [SerializeField]
    private Canvas uiCanvas;

    private float nextSpawnTime;

    private Queue<GameObject> activeObstacles;

    private Queue<GameObject> passedObstacles;

    [SerializeField]
    private TextMeshProUGUI scoreUi;

    [SerializeField]
    private TextMeshProUGUI pauseButtonText;

    [SerializeField]
    private TextMeshProUGUI scoreTextValue;

    [SerializeField]
    private TextMeshProUGUI highScoreTextValue;

    [SerializeField]
    private TextMeshProUGUI newText;

    [SerializeField]
    private UnityEngine.UI.Button pauseButton;

    [SerializeField]
    private Sprite[] medalImages;

    [SerializeField]
    private Image displayedMedal;

    private bool birdDead;

    private int score;

    private bool isPaused;

    private const int passedLimit = 5;

    private const int medalOffset = 10;

    void Start()
    {

        nextSpawnTime = spawnDelay;
        activeObstacles=new Queue<GameObject>();
        passedObstacles = new Queue<GameObject>();
       
        birdDead = false;
        score = 0;
        scoreUi.SetText("0");
        SpawnNextObstacle();
        isPaused = false;

    }

    void Update()
    {
      
        if (!birdDead && !isPaused)
        {
            foreach (GameObject activeObstacle in activeObstacles)
            {
                activeObstacle.transform.position -= new Vector3(worldSpeed, 0f, 0f) * Time.deltaTime;
  
            }

            foreach (GameObject passedObstacle in passedObstacles)
            {
                passedObstacle.transform.position -= new Vector3(worldSpeed, 0f, 0f) * Time.deltaTime;
            }

            if (activeObstacles.Peek().transform.position.x<= birdTransform.position.x)
            {
                passedObstacles.Enqueue(activeObstacles.Dequeue());
                score++;
                scoreUi.SetText(score.ToString());
            }

            
            nextSpawnTime -= Time.deltaTime;

            if (nextSpawnTime <= 0f)
            {
                nextSpawnTime = spawnDelay;

                SpawnNextObstacle();
            }

        }
    }

    private void SpawnNextObstacle()
    {
        System.Random random = new();

        float heightValue = (float) random.Next(0, 12) / 4;

        GameObject spawned;

        if (passedObstacles.Count>= passedLimit)
        {
            spawned = passedObstacles.Dequeue();
            spawned.transform.position = new Vector3(startingX, heightValue,0);
            
        }
        else
        {
            spawned = Instantiate(obstacleEasy, new Vector3(startingX, heightValue, 0), Quaternion.identity);
        }

        activeObstacles.Enqueue(spawned);

    }


    public void HandleDeath()
    {
        worldSpeed = 0f;
        birdDead = true;

        int currentHighScore = PlayerPrefs.GetInt("Highscore");

        deathMenu.gameObject.SetActive(true);
        uiCanvas.gameObject.SetActive(false);

        if (currentHighScore < score)
        {
            currentHighScore = score;
            PlayerPrefs.SetInt("Highscore", this.score);
        }
        else
        {
            newText.gameObject.SetActive(false);
        }


        int medalIndex = score / medalOffset - 1;

        if (medalIndex >= 0)
        {
            medalIndex = medalIndex > medalImages.Length - 1 ? medalImages.Length - 1 : medalIndex;

            displayedMedal.sprite = medalImages[medalIndex];
        }

        scoreTextValue.SetText(score.ToString());
        highScoreTextValue.SetText(currentHighScore.ToString());

    }

    public void PausePressed()
    {

        isPaused = !isPaused;
        pauseButtonText.SetText(isPaused ? "Resume" : "Pause");
    }

    public bool IsPaused()
    {
        return isPaused;
    }


    public void ToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
