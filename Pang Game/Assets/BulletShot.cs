using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletShot : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    [SerializeField] private float speed = 4f;

    void Awake() => InitVars();

    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        m_Rigidbody.velocity = new Vector2(0f, speed);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        bool isTouchingAnyBall = (coll.tag == "BallSize_1" ||
            coll.tag == "BallSize_2" ||
            coll.tag == "BallSize_3" ||
            coll.tag == "BallSize_4" ||
            coll.tag == "BallSize_5" ||
            coll.tag == "BallSize_6");

        if (isTouchingAnyBall)
            Debug.Log("ball & shot destroyed");
        else if (coll.tag == "Ceil")
            Debug.Log("shot destroyed");
    }
}
