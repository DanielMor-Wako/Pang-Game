using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;

    // ID holds ref to the player index number on App.Model.Player[index]
    // player1 index = 0 || player2 index = 1
    public int ID;

    void Awake() => InitVars();

    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // called from GameManager on fixed update
    public void PlayerMove(float x)
    {
        float force = 0f;
        float velocity = Mathf.Abs(m_Rigidbody.velocity.x);

        float xMove = x;
        
        // player wish to move left or right
        if (xMove != 0)
        {
            // if player has not reached max speed, and can still move
            // move on x Axis based on keyboard input (-1 = left, 1 = right)
            if (velocity < AppModel._instance.player[ID].maxVelocity)
                force = AppModel._instance.player[ID].speed * Mathf.Sign(xMove);

            m_Rigidbody.AddForce(new Vector2(force, 0));

            // changing player x scale to face left or right
            FlipCharacter(Mathf.Sign(xMove));
        }
    }

    // flips character horizontally based on xMove input (-1 = left, 1 = right)
    void FlipCharacter(float newDirection)
    {
        Vector3 scale = transform.localScale;
        if (newDirection != scale.x)
        {
            scale.x = newDirection;
            transform.localScale = scale;
        }
    }

    public void OnPlayerDeath()
    {
        m_Rigidbody.transform.position = new Vector2(-100, 0);

        gameObject.SetActive(false);

        GameManager._instance.NotifyPlayerDied(ID);
    }
}