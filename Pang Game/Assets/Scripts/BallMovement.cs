using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody2D m_Rigidbody;
    public Vector2 velo; // stored reference for ball velocity

    [Range(-1, 1)] [SerializeField] private int xDirection;

    void Awake() => InitVars();


    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();

        SetBallSpeed();
    }

    public void SetBallSpeed()
    {
        // sets ball speed on initilization
        // horizontal as fixed float
        // vertical speed varied based on the ball size
        float xSpeed = AppModel._instance.levels[AppModel._instance.game.currentLevel].BallsXspeed;
        float ySpeed = 0f;

        switch (this.gameObject.tag)
        {
            case "BallSize_6":
                ySpeed = 11f;
                break;

            case "BallSize_5":
                ySpeed = 11f;
                break;

            case "BallSize_4":
                ySpeed = 10f;
                break;

            case "BallSize_3":
                ySpeed = 9f;
                break;

            case "BallSize_2":
                ySpeed = 8f;
                break;

            case "BallSize_1":
                ySpeed = 7.5f;
                break;

        }

        velo = new Vector2(xSpeed, ySpeed);
    }

    // This function gives initial jump to the ball,
    // it is called when starting a new level AND when a new ball is spawned during the game
    public void InitialJump()
    {
        m_Rigidbody.velocity = new Vector2(0, 3.5f);
    }
    public void SetBallxDirection(int newDirection)
    {
        xDirection = newDirection;
    }

    private void Update()
    {
        if (!GameManager._instance.isGameRunning)
            return;

        MoveBall();
    }

    void MoveBall()
    {
        // dont move the ball left or right when no direcion has been set
        if (xDirection == 0)
            return;

        // moving the transform.position based on the our set xDirection(-1,0,1) * by speed.x
        Vector3 newPos = m_Rigidbody.position;
        newPos.x += (velo.x * xDirection) * Time.deltaTime;
        m_Rigidbody.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Ground")
            m_Rigidbody.velocity = new Vector2(0, velo.y);
        else if (coll.tag == "Player")
        {
            CharacterController2D controller = coll.GetComponent<CharacterController2D>();
            controller?.OnPlayerDeath();
            GetComponent<BallPop>()?.PopBall();
        }
        else if (coll.tag == "Shot")
        {
            coll.gameObject.SetActive(false);
            GetComponent<BallPop>()?.PopBall();
        }
        else if (coll.tag == "WallRight")
            xDirection = -1;
        else if (coll.tag == "WallLeft")
            xDirection = 1;
        else if (coll.tag == "Ceil")
            m_Rigidbody.velocity = Vector2.zero;
    }
    
}
