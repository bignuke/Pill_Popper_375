﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;
    public RectTransform newWaveBanner;
    public RectTransform healthBar;
    public Text newWaveTitle;
    public Text newWaveEnemyCount;
    public Text scoreUI;
    public Text highscoreValUI;
    public Text gameOverScoreUI;
    Spawner spawner;
    Player player;

    void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        spawner.onNewWave += onNewWave;
        player = FindObjectOfType<Player>();
    }

	void Start () {
        FindObjectOfType<Player>().onDeath += onGameOver;
        highscoreValUI.text = PlayerPrefs.GetInt("Highscore1").ToString();
	}
	
	void Update () {
        scoreUI.text = ScoreKeeper.score.ToString("D6");
        float healthPercent = 0;
        if(player != null)
        {
            healthPercent = player.health / player.startingHealth;  
        }
        healthBar.localScale = new Vector3(healthPercent * 2, 1, 1);
    }

    void onGameOver()
    {
        int score = System.Int32.Parse(scoreUI.text);

        StartCoroutine(fade(Color.clear, new Color(0, 0, 0, 0.95f), 1));
        scoreUI.gameObject.SetActive(false);
        highscoreValUI.gameObject.SetActive(false);
        healthBar.parent.gameObject.SetActive(false);
        gameOverScoreUI.text = scoreUI.text;
        gameOverUI.SetActive(true);
        updateHighScores(score);
    }

    public void updateHighScores(int score)
    {
        int highscore1 = PlayerPrefs.GetInt("Highscore1");
        int highscore2 = PlayerPrefs.GetInt("Highscore2");
        int highscore3 = PlayerPrefs.GetInt("Highscore3");
        int highscore4 = PlayerPrefs.GetInt("Highscore4");
        int highscore5 = PlayerPrefs.GetInt("Highscore5");

        if (score > highscore5)
        {
            PlayerPrefs.SetInt("Highscore5", score);

            if (score > highscore4)
            {
                int tempScore = 0;
                tempScore = PlayerPrefs.GetInt("Highscore4");
                PlayerPrefs.SetInt("Highscore5", tempScore);
                PlayerPrefs.SetInt("Highscore4", score);

                if (score > highscore3)
                {
                    tempScore = PlayerPrefs.GetInt("Highscore3");
                    PlayerPrefs.SetInt("Highscore4", tempScore);
                    PlayerPrefs.SetInt("Highscore3", score);

                    if (score > highscore2)
                    {
                        tempScore = PlayerPrefs.GetInt("Highscore2");
                        PlayerPrefs.SetInt("Highscore3", tempScore);
                        PlayerPrefs.SetInt("Highscore2", score);

                        if (score > highscore1)
                        {
                            tempScore = PlayerPrefs.GetInt("Highscore1");
                            PlayerPrefs.SetInt("Highscore2", tempScore);
                            PlayerPrefs.SetInt("Highscore1", score);
                        }
                    }
                }
            }
        }
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void onNewWave(int waveNumber)
    {
        string[] numbers = { "One", "Two", "Three", "Four", "Five"};
        newWaveTitle.text = "- Wave " + numbers[waveNumber - 1] + " -";
        string enemyCountString = ((spawner.waves[waveNumber - 1]).infinite ? "Infinite" : spawner.waves[waveNumber - 1].enemyCount + "");
        newWaveEnemyCount.text = "Enemies: " + enemyCountString;
        StopCoroutine("animateNewWave");
        StartCoroutine("animateNewWave");
    }

    IEnumerator animateNewWave()
    {
        float percent = 0;
        float speed = 2.5f;
        float delayTime = 1f;
        float endDelayTime = Time.time + (1 / speed) + delayTime;
        int direction = 1;

        while(percent >= 0)
        {
            percent += Time.deltaTime * speed * direction;
            if(percent >= 1)
            {
                percent = 1;
                if(Time.time > endDelayTime)
                {
                    direction = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-170, 45, percent);
            yield return null;
        }
    }

    IEnumerator fade(Color initColor, Color fadeTo, int time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(initColor, fadeTo, percent);
            yield return null;
        }
    }

    //Input
    public void startNewGame()
    {
        SceneManager.LoadScene("Game");
    }
}
