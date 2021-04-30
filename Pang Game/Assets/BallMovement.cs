using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody2D m_Rigidbody;
    public Vector2 speed;

    [Range(-1, 1)] [SerializeField] private int xDirection;

    void Awake() => InitVars();

    private void Update()
    {
        MoveBall();
    }

    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();

        SetBallSpeed();
    }

    public void SetBallxDirection(int newDirection)
    {
        xDirection = newDirection;
    }

    void SetBallSpeed()
    {
        // sets ball speed on initilization
        // horizontal as fixed float
        // vertical speed varied based on the ball size
        float xSpeed = 3f;
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
                ySpeed = 7f;
                break;

        }

        speed = new Vector2(xSpeed, ySpeed);
    }

    void MoveBall()
    {
        // dont move the ball left or right when no direcion has been set
        if (xDirection == 0)
            return;

        // moving the transform.position based on the our set xDirection(-1,0,1) * by speed.x
        Vector3 newPos = m_Rigidbody.position;
        newPos.x += (speed.x * xDirection) * Time.deltaTime;
        m_Rigidbody.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Ground")
            m_Rigidbody.velocity = new Vector2(0, speed.y);
        else if (coll.tag == "Player")
            Debug.Log("player died");
        else if (coll.tag == "WallRight")
            xDirection = -1;
        else if (coll.tag == "WallLeft")
            xDirection = 1;
        else if (coll.tag == "Ceil")
            m_Rigidbody.velocity = Vector2.zero;
    }

}
