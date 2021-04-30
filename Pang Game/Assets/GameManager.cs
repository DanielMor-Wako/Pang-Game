using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isGameRunning = false;
    public int totalPlayers = 2;
    private int alivePlayers = 2;
    private float Countdown = 0;
    [SerializeField] int score = 0;
    [SerializeField] CharacterController2D player1;
    [SerializeField] CharacterController2D player2;

    //[SerializeField] CharacterController2D controller;
    private Camera cam;
    private UiMenusManager uiMenuManager;
    public LevelSet[] LevelsList;
    public int currentLevel;

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

        if (uiMenuManager == null)
            uiMenuManager = GameObject.FindObjectOfType<UiMenusManager>();
    }

    public void Start()
    {
        //currentLevel = Random.Range(1, LevelsList.Length);
        //StartLevel(currentLevel);
    }

    public void PlayerDied(int PlayerID)
    {
        alivePlayers--;
        bool noPlayersLeft = isGameOver();
        if (noPlayersLeft)
            GameOver();
    }
    public bool isGameOver()
    {
        bool result;
        result = alivePlayers <= 0;
        Debug.Log("isGameOver = " + result);
        return result;
    }
    private void GameOver()
    {
        Debug.Log("Game Over");
        uiMenuManager.Activate("mainMenu");
    }
    public void StartNewGame(int playersCount)
    {
        Debug.Log("New Game");
        totalPlayers = playersCount;
        alivePlayers = totalPlayers;

        currentLevel = 1;
        StartLevel(currentLevel);
}
    public void StartNextLevel()
    {
        Debug.Log("Start next level");
    }
    public void AbandonGame()
    {
        Debug.Log("Stop Game");
    }
    public void QuitApp()
    {
        Application.Quit();
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

        // clear all ui screens
        uiMenuManager.Clear();
        //StopCoroutine("LevelSpawner");
        StartCoroutine("LevelSpawner", LevelToLoad);
    }

    IEnumerator LevelSpawner(int LevelID)
    {
        // load new game by the level index
        LevelSet incomingLevel = LevelsList[LevelID];

        isGameRunning = false;
        // clear previous objects
        ObjectPoolList._instance.DespawnAll();

        // spawn player/s
        alivePlayers = totalPlayers;
        Debug.Log("new game with "+ alivePlayers+" player");
        if (totalPlayers == 2)
        {
            player1.transform.position = incomingLevel.Player1SpawnPoint.position;
            player1.gameObject.SetActive(true);
            player2.transform.position = incomingLevel.Player2SpawnPoint.position;
            player2.gameObject.SetActive(true);
        }
        else if (totalPlayers == 1)
        {
            player1.gameObject.SetActive(true);
            player1.transform.position = incomingLevel.Player1SpawnPoint.position;
            player2.gameObject.SetActive(false);
        }
        Countdown = incomingLevel.TimerCountdown;
        // change background
        //backgroundImage.SwitchToSprite(level);
        // generate level
        List<BallMovement> NewBallsList = new List<BallMovement>();
        for (int i=0; i < incomingLevel.Balls.Length; i++)
        {
            // spawn balls
            GameObject NewBall = ObjectPoolList._instance.SpawnObject(incomingLevel.Balls[i].BallSize.ToString(), incomingLevel.Balls[i].BallSpawnPoint.position);
            // change ball xDirection
            BallMovement NewBallScript = NewBall.GetComponent<BallMovement>();
            if (NewBallScript != null)
            {
                NewBallScript.SetBallxDirection(incomingLevel.Balls[i].BallInitialDirection);
                NewBallScript.m_Rigidbody.simulated = false;
                NewBallsList.Add( NewBallScript );
                
            }
        }

        float DelayBeforeLevelStarts = 3f;
        StartIncomingLevelAnimation(true, DelayBeforeLevelStarts);

        yield return new WaitForSeconds(DelayBeforeLevelStarts);

        StartIncomingLevelAnimation(false, 0f);
        isGameRunning = true;
        
        // give all balls an initial jump
        foreach (BallMovement bm in NewBallsList.ToArray())
        {
            bm.m_Rigidbody.simulated = true;
            bm.InitialJump();
        }

        Debug.Log("LevelSpawner(" + incomingLevel.LevelID + ") Ended ");
    }
    
    private void StartIncomingLevelAnimation(bool activate, float countdown)
    {
        if (uiMenuManager == null)
            return;

        LoadingLevelUpdater LoadingLevelScreen = uiMenuManager.levelLoading.GetComponent<LoadingLevelUpdater>();

        LoadingLevelScreen.gameObject.SetActive(activate);
        LoadingLevelScreen.countdown = countdown;
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
