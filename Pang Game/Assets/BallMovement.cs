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

        SetBallSpeedY();
    }

    void SetBallSpeedY()
    {
        float xSpeed = 2.5f;

        switch (this.gameObject.tag)
        {
            case "BallSize_6":
            break;

            case "BallSize_5":
                break;

            case "BallSize_4":
                break;

            case "BallSize_3":
                break;

            case "BallSize_2":
                break;

            case "BallSize_1":
                break;

        }
        float ySpeed = 2.5f;

        speed = new Vector2(xSpeed, ySpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
