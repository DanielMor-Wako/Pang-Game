using UnityEngine;


public class AppModel:MonoBehaviour
{
    public PlayerModel[] player;
    public GameModel game;
    public LevelsModel[] levels;

    #region SINGLETON PATTERN
    public static AppModel _instance;
    public static AppModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AppModel>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("AppModel");
                    _instance = container.AddComponent<AppModel>();
                }
            }

            return _instance;
        }
    }
    #endregion
    private void Awake()
    {
        #region SINGLETON PATTERN
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        #endregion
    }
}

[System.Serializable]
public class GameModel
{
    // number of current level and total incremented score
    public int currentLevel = 0;
    public int score = 0;

    // player popped all balls on the screen
    public int ballsLeft = 0;
    public int winCondition = 0;

    // player lose due to time restriction or when none players left
    [Range(1,2)] public int totalPlayers = 1;
    public int playersAlive = 1;
    public float CountdownLeft = 30;
    public int LoseCondition = 0; 
}

[System.Serializable]
public class PlayerModel
{
    public GameObject playerPrefab;

    public float speed = 5f;
    public float maxVelocity = 5f;
    public int lives = 1;

    public float m_moveDelay = 0.2f;
    public bool b_canMove = true;
    public float m_shootDelay = 0.5f;
    public bool b_canShoot = true;

    public WeaponModel weapon;
}

[System.Serializable]
public class WeaponModel
{
    public enum WeaponPrefab { Rope, Shot, StickyRope, Laser }
    public WeaponPrefab activeWeapon;

    // max active shots that can exist simultaionasly on the scene
    [Range(1, 5)] [SerializeField] public int maxActiveShots = 1;

    // max duration of sticky weapons (that sticks to the ceiling), 0 = none;
    [Range(0, 5)] [SerializeField] public int maxStickTime = 0;
}

[System.Serializable]
public class LevelsModel
{
    public int LevelID;
    public float TimerCountdown = 20.0f;
    public Transform Player1SpawnPoint, Player2SpawnPoint;
    public BallModel[] Balls;
    [Range(2, 10)] public float BallsXspeed = 5f; // this moves the ball on x axis, faster rate increases the difficulty

    public LevelsModel
        (
        int LevelID,
        float TimerCountdown,
        Transform Player1SpawnPoint,
        Transform Player2SpawnPoint,
        BallModel[] Balls,
        float BallsXspeed
        )
    {
        this.LevelID = LevelID;
        this.TimerCountdown = TimerCountdown;
        this.Player1SpawnPoint = Player1SpawnPoint;
        this.Player2SpawnPoint = Player2SpawnPoint;
        this.Balls = Balls;
        this.BallsXspeed = BallsXspeed;
    }
}

[System.Serializable]
public class BallModel
{
    public Transform BallSpawnPoint;

    public enum BallPrefab { None, BallSize_1, BallSize_2, BallSize_3, BallSize_4, BallSize_5, BallSize_6 }
    public BallPrefab BallSize;
    [Range(-1, 1)] public int BallInitialDirection;

    public BallModel
        (
        Transform BallSpawnPoint,
        BallPrefab BallSize,
        int BallInitialDirection
        )
    {
        this.BallSpawnPoint = BallSpawnPoint;
        this.BallSize = BallSize;
        this.BallInitialDirection = BallInitialDirection;
    }
}