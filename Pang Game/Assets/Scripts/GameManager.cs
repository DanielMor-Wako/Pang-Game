using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // reference to players input
    [HideInInspector] PlayerInput playersInput;

    // reference to the player gameobjects
    [SerializeField] CharacterController2D player1;
    [SerializeField] CharacterController2D player2;

    // reference to the player shooting by objectpool
    private string player1_weaponPool;
    private string player2_weaponPool;

    private Camera cam;
    private UiMenusManager uiMenuManager;

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

        Initilize();
    }

    void Initilize()
    {
        if (cam == null)
            cam = Camera.main;

        if (playersInput == null)
            playersInput = GetComponent<PlayerInput>();

        if (uiMenuManager == null)
            uiMenuManager = GetComponent<UiMenusManager>();

        // these refer as reference string for the players to initiate shots,
        // it needs to be called again if a player has changed its weapon
        player1_weaponPool = "Player1" + AppModel._instance.player[0].weapon.activeWeapon.ToString();
        player2_weaponPool = "Player2" + AppModel._instance.player[1].weapon.activeWeapon.ToString();
    }
    
    // Player Inputs and output to modal.view
    private void FixedUpdate()
    {
        // cancel moving characters when game is not running
        if (!isGameRunning)
            return;
        
        // call move & shoot functions on each alive player
        if (AppModel._instance.player[0].lives > 0)
        {
            if (AppModel._instance.player[0].b_canMove)
                player1.PlayerMove(playersInput.GetPlayer1XInput());
            if (AppModel._instance.player[0].b_canShoot)
                CheckPlayerShooting(0);
        }
        if (AppModel._instance.player[1].lives > 0)
        {
            if (AppModel._instance.player[1].b_canMove)
                player2.PlayerMove(playersInput.GetPlayer2XInput());
            if (AppModel._instance.player[1].b_canShoot)
                CheckPlayerShooting(1);
        }
    }
    // Player's shooting input
    private void CheckPlayerShooting(int PlayerID)
    {
        // get the input by the PlayeriD
        float yInput;
        if (PlayerID == 1)
            yInput = playersInput.GetPlayer2YInput();
        else
            yInput = playersInput.GetPlayer1YInput();

        // if player wish to shoot
        if (yInput > 0)
        {
            // check if any shots slots are avaiable
            bool allowShooting = IsShootingAllowed(PlayerID);
            if (allowShooting)
            {
                // shoot the active weapon of the player
                StartPlayerShooting(PlayerID);
            }
        }
    }
    bool IsShootingAllowed(int PlayerID)
    {
        bool result = true;

        ObjectPool activeWeaponPool = null;
        // searching for our weapon prefab based on the player ID and active weapon. example "Player2Shot" "Player1Laser"
        if (activeWeaponPool == null)
        {
            string weaponPoolName = "Player" + (PlayerID + 1) + AppModel._instance.player[PlayerID].weapon.activeWeapon.ToString();
            activeWeaponPool = ObjectPoolList._instance.GetRelevantPool(weaponPoolName);
        }

        if (activeWeaponPool != null)
        {
            // checking if any new slot for a shot is available (-1 = none, 0 = it has 1 in the array)
            int activeShots = activeWeaponPool.ReturnActiveObjectsCount();
            int avaialbleSlots = AppModel._instance.player[PlayerID].weapon.maxActiveShots - activeShots;
            //Debug.Log("NewAvaialbleSlots " + avaialbleSlots);
            if (avaialbleSlots <= 0)
                result = false;
        }

        return result;
    }
    // Deal with player's spawning of new shots by reference to objectpool component
    private string GetPlayerWeaponPool(int PlayerID)
    {
        if (PlayerID == 1)
            return player2_weaponPool;
        else
            return player1_weaponPool;
    }
    private Vector2 GetPlayerPos(int PlayerID)
    {
        Vector2 newPos;
        if (PlayerID == 1)
            newPos = player2.transform.position;
        else 
            newPos = player1.transform.position;

        return newPos;
    }
    public void StartPlayerShooting(int PlayerID)
    {
        // Start shooting coroutine
        StartCoroutine(ShootCoroutine(PlayerID));
    }
    private IEnumerator ShootCoroutine(int PlayerID)
    {
        AppModel._instance.player[PlayerID].b_canShoot = false;
        AppModel._instance.player[PlayerID].b_canMove = false;
        //string weaponPoolName = "Player" + (PlayerID + 1) + AppModel._instance.player[PlayerID].weapon.activeWeapon.ToString();
        //activeWeaponPool = ObjectPoolList._instance.GetRelevantPool(weaponPoolName);
        string weaponPoolName = GetPlayerWeaponPool(PlayerID);
        Vector2 newPos = GetPlayerPos(PlayerID);
        ObjectPoolList._instance.SpawnObject(weaponPoolName, newPos);

        // wait the shooting delay time and then continue
        yield return new WaitForSeconds(AppModel._instance.player[PlayerID].m_moveDelay);

        AppModel._instance.player[PlayerID].b_canMove = true;

        // wait the shooting delay time and then continue
        yield return new WaitForSeconds(AppModel._instance.player[PlayerID].m_shootDelay);

        AppModel._instance.player[PlayerID].b_canShoot = true;
    }
    
    // Notifications
    public void NotifyPlayerDied(int PlayerID)
    {
        AppModel._instance.player[PlayerID].lives = 0;
        AppModel._instance.game.playersAlive--;
        bool noPlayersLeft = isGameOver();
        if (noPlayersLeft)
            GameOver();
    }
    public void NotifyBallPopped(string ballTag)
    {
        Debug.Log(ballTag + " has poped");
        AppModel._instance.game.ballsLeft --;
        bool noBallsLeft = isLevelComplete();
        if (noBallsLeft)
            LevelComplete();
    }

    // Game states
    public bool isLevelComplete()
    {
        bool result;
        result = AppModel._instance.game.ballsLeft <= 0;
        return result;
    }
    public void LevelComplete()
    {
        StartNextLevel();
    }
    public bool isGameOver()
    {
        bool result;
        result = AppModel._instance.game.playersAlive <= 0;
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
        AppModel._instance.game.totalPlayers = playersCount;
        AppModel._instance.game.playersAlive = playersCount;
        AppModel._instance.game.currentLevel = 1;
        StartLevel(1);
}
    public void StartNextLevel()
    {
        Debug.Log("Start next level");
        AppModel._instance.game.currentLevel ++;
        StartLevel(AppModel._instance.game.currentLevel);
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
    
    // Access data for the next loaded level
    public void StartLevel(int newLvl)
    {
        //Debug.Log("Level starter " + newLvl + " is triggered");

        // find the current Level to start from the array
        int LevelToLoad = -1;
        //foreach (var LevelID in LevelsList)
        for (var LevelID = 0; LevelID < AppModel._instance.levels.Length; LevelID++)
        {
            //Debug.Log("checking against " + LevelsList[LevelID].LevelID);
            if (newLvl == AppModel._instance.levels[LevelID].LevelID)
            {
                LevelToLoad = LevelID;
                Debug.Log("Loading Level " + AppModel._instance.levels[LevelID].LevelID + " id(" + LevelToLoad + ") ");
            }
        }

        if (newLvl > AppModel._instance.levels.Length)
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
    // Generating the level based on AppModel._instance.levels[LevelToLoad]
    IEnumerator LevelSpawner(int LevelID)
    {
        // load new game by the level index
        LevelsModel incomingLevel = AppModel._instance.levels[LevelID];

        isGameRunning = false;
        // clear previous objects
        ObjectPoolList._instance.DespawnAll();

        // spawn player/s
        AppModel._instance.game.playersAlive = AppModel._instance.game.totalPlayers;
        Debug.Log("new game with "+ AppModel._instance.game.totalPlayers+ " player");
        if (AppModel._instance.game.totalPlayers == 2)
        {
            // player 1 is active
            player1.transform.position = incomingLevel.Player1SpawnPoint.position;
            player1.gameObject.SetActive(true);
            AppModel._instance.player[0].b_canMove = true;
            AppModel._instance.player[0].b_canShoot = true;
            AppModel._instance.player[0].lives = 1;
            // player 2 is active
            player2.transform.position = incomingLevel.Player2SpawnPoint.position;
            player2.gameObject.SetActive(true);
            AppModel._instance.player[1].b_canMove = true;
            AppModel._instance.player[1].b_canShoot = true;
            AppModel._instance.player[1].lives = 1;
        }
        else if (AppModel._instance.game.totalPlayers == 1)
        {
            // player 1 is active
            player1.gameObject.SetActive(true);
            player1.transform.position = incomingLevel.Player1SpawnPoint.position;
            AppModel._instance.player[0].b_canMove = true;
            AppModel._instance.player[0].b_canShoot = true;
            AppModel._instance.player[0].lives = 1;
            // player 2 is not active
            player2.gameObject.SetActive(false);
            AppModel._instance.player[1].lives = 0;
        }
        AppModel._instance.game.CountdownLeft = incomingLevel.TimerCountdown;
        // change background
        //backgroundImage.SwitchToSprite(level);
        // generate level
        int totalBallsCount = 0;
        List<BallMovement> NewBallsList = new List<BallMovement>();
        for (int i=0; i < incomingLevel.Balls.Length; i++)
        {
            // spawn balls
            GameObject NewBall = ObjectPoolList._instance.SpawnObject(incomingLevel.Balls[i].BallSize.ToString(), incomingLevel.Balls[i].BallSpawnPoint.position);
            // change ball properties
            BallMovement NewBallScript = NewBall.GetComponent<BallMovement>();
            if (NewBallScript != null)
            {
                NewBallScript.SetBallSpeed(); // this sets the internall vector2 'velo' that moves the ball to the current level speed
                NewBallScript.SetBallxDirection(incomingLevel.Balls[i].BallInitialDirection);
                NewBallScript.m_Rigidbody.simulated = false;
                NewBallsList.Add( NewBallScript );
            }
            // add ball as collective sum of inner balls to calculate total balls count in the level
            totalBallsCount += GetTotalBallsCountBySize(incomingLevel.Balls[i].BallSize);
        }
        // saves the level's total balls count to check against win condition
        AppModel._instance.game.ballsLeft = totalBallsCount;

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
    private int GetTotalBallsCountBySize(BallModel.BallPrefab ballsize)
    {
        int totalCount = 0;

        switch (ballsize)
        {
            case BallModel.BallPrefab.BallSize_1:
                totalCount = 1;
                break;

            case BallModel.BallPrefab.BallSize_2:
                totalCount = 3;
                break;

            case BallModel.BallPrefab.BallSize_3:
                totalCount = 7;
                break;

            case BallModel.BallPrefab.BallSize_4:
                totalCount = 15;
                break;

            case BallModel.BallPrefab.BallSize_5:
                totalCount = 31;
                break;

            case BallModel.BallPrefab.BallSize_6:
                totalCount = 63;
                break;

        }
        return totalCount;
    }
    private void StartIncomingLevelAnimation(bool activate, float countdown)
    {
        if (uiMenuManager == null)
            return;

        LoadingLevelUpdater LoadingLevelScreen = uiMenuManager.levelLoading.GetComponent<LoadingLevelUpdater>();
        LoadingLevelScreen.LevelLoaderDisplay(AppModel._instance.game.currentLevel, countdown);
        LoadingLevelScreen.countdown = countdown;
        LoadingLevelScreen.gameObject.SetActive(activate);
    }

    // player score
    public int GetScore()
    {
        return AppModel._instance.game.score;
    }
    public void AddScore(int increment)
    {
        AppModel._instance.game.score += increment;
        //scoreTxt.text = score.ToString();
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
}
