using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody2D m_Rigidbody;
    public Vector2 speed;

    [Range(-1, 1)]
    public int xDirection;

    void Awake() => InitVars();


    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();

        SetBallSpeed();
    }
    
    void SetBallSpeed()
    {
        // sets ball speed on initilization
        // horizontal as fixed float
        // vertical speed varied based on the ball size
        float xSpeed = 2.5f;
        float ySpeed = 0f;

        switch (this.gameObject.tag)
        {
            case "BallSize_6":
                ySpeed = 12f;
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

    
}
