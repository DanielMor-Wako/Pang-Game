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

    private bool CheckCollisionWithAnyBall(string theTag)
    {
        // we check collision with balls using splitfunction of the incoming tag name
        // if there is a "BallSize_" + (any number).. meaning we collided with a ball object.. then function returns true
        bool result = false;
        string[] collTag = theTag.Split('_');
        if (collTag[0] == "BallSize")
            result = true;
        return result;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        bool isTouchingAnyBall = CheckCollisionWithAnyBall(coll.tag);

        if (isTouchingAnyBall)
        {
            Debug.Log("ball & shot destroyed");
            coll.GetComponent<BallPop>()?.PopBall();
        }
        else if (coll.tag == "Ceil")
            gameObject.SetActive(false);
    }
}
