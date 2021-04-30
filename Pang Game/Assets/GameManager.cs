﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;

[System.Serializable]
public class Ball_Properties
{
    public Transform BallSpawnPoint;
    public BallPop.BallPrefab BallSize;
    [Range(-1, 1)] public int BallInitialDirection;

    public Ball_Properties
        (
        Transform BallSpawnPoint,
        BallPop.BallPrefab BallSize,
        int BallInitialDirection
        )
    {
        this.BallSpawnPoint = BallSpawnPoint;
        this.BallSize = BallSize;
        this.BallInitialDirection = BallInitialDirection;
    }
}

[System.Serializable]
public class LevelSet
{
    public int LevelID;// { get; set; }
    public float TimerCountdown = 20.0f;
    public Transform Player1SpawnPoint, Player2SpawnPoint;
    public Ball_Properties[] Balls;

    public LevelSet
        (
        int LevelID,
        float TimerCountdown,
        Transform Player1SpawnPoint,
        Transform Player2SpawnPoint,
        Ball_Properties[] Balls
        )
    {
        this.LevelID = LevelID;
        this.TimerCountdown = TimerCountdown;
        this.Player1SpawnPoint = Player1SpawnPoint;
        this.Player2SpawnPoint = Player2SpawnPoint;
        this.Balls = Balls;
    }
}


public class GameManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("GameManager");
                    _instance = container.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    #endregion
    private int Countdown = 0;
    [SerializeField] int score = 0;
    //[SerializeField] TextMeshProUGUI scoreTxt;

    //[SerializeField] CharacterController2D controller;
    private Camera cam;
    public LevelSet[] LevelsList;

    private void Awake()
    {
        #region SINGLETON PATTERN
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        if (cam == null)
            cam = Camera.main;
    }

    public void Start()
    {
        
    }

    public bool isGameOver()
    {
        return true;
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
        // uiMenuManager.Activate("level...");
    }
    public void StartNewGame()
    {
        Debug.Log("New Game");
        // load new game by the level index
        // load player/s , ball/s and switch background
        //backgroundImage.SwitchToSprite(level);
    }
    public void StartNextLevel()
    {
        Debug.Log("Start next level");
    }
    public void AbandonGame()
    {
        Debug.Log("Stop Game");
    }
    public void PauseMenu(bool activate)
    {
        if (activate)
        {
            // Pause the game
            Debug.Log("Pause Game");
        }
        else
        {
            // Unpause the game
            Debug.Log("Unpause Game");
        }
    }
    public void PostHighscore(string username)
    {
        Debug.Log("posting high score for "+username);
        //Highscores.AddNewHighscore(username, score);
    }
    public void OnHighscorePosted()
    {
        Debug.Log("posted high score");
        //uiMenuManager.Activate("tophighscores");
    }
    
    public void StartLevel(int newLvl)
    {
        //Debug.Log("Level starter " + newLvl + " is triggered");

        // find the current Level to start
        int LevelToLoad = -1;
        //foreach (var LevelID in LevelsList)
        for (var LevelID = 0; LevelID < LevelsList.Length; LevelID++)
        {
            //Debug.Log("checking against " + LevelsList[LevelID].LevelID);
            if (newLvl == LevelsList[LevelID].LevelID)
            {
                //LevelToLoad = newLvl;//LevelsList[LevelID].LevelID;
                LevelToLoad = LevelID;//LevelsList[LevelID].LevelID;
                Debug.Log("Loading Level " + LevelsList[LevelID].LevelID + " id(" + LevelToLoad + ") ");
            }
        }

        if (newLvl > LevelsList.Length)
        {
            Debug.Log("No More levels, Game is complete!");
            return;
        }

        if (LevelToLoad < 0)
        {
            Debug.Log("failed to find new Level data");
            return;
        }

        // add objects
        //StopCoroutine("LevelSpawner");
        StartCoroutine("LevelSpawner", LevelToLoad);
    }

    IEnumerator LevelSpawner(int LevelID)
    {
        LevelSet incomingLevel = LevelsList[LevelID];

        StartIncomingLevelAnimation();
        yield return new WaitForSeconds(1f);
        
        Debug.Log("LevelSpawner(" + incomingLevel.LevelID + ") Ended ");
    }
    
    private void StartIncomingLevelAnimation()
    {
        Debug.Log("Ready... set... go");
        //incomingLevelAnim.gameObject.SetActive(true);
    }
    public int GetScore()
    {
        return score;
    }
    
    public void AddScore(int increment)
    {
        score += increment;
        //scoreTxt.text = score.ToString();
    }
}
