using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController2D : MonoBehaviour
{
    // init player vars
    [SerializeField] float speed = 5f;
    [SerializeField] float maxVelocity = 5f;

    PlayerInput playerInput;
    Rigidbody2D m_Rigidbody;
    // TODO 
    //private Animator anim;
    //private AudioClip shootSound;

    private bool b_canMove, b_canShoot;
    

    void Awake() => InitVars();

    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();

        if (playerInput == null)
            playerInput = Object.FindObjectOfType<PlayerInput>();

        b_canMove = true;
        b_canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void MovePlayer()
    {
        float force = 0f;
        float velocity = Mathf.Abs(m_Rigidbody.velocity.x);

        Vector2 move = playerInput.GetPlayerInput();
        
        if (b_canMove)
        {
            // player wish to move left or right
            if (move.x != 0)
            {
                // if player has not reached max speed, and can still move
                // move on x Axis based on keyboard input
                if (velocity < maxVelocity)
                    force = speed * Mathf.Sign(move.x);
            }
        }

    }
}