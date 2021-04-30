using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController2D : MonoBehaviour
{
    // init player vars
    [SerializeField] float speed = 5f;
    [SerializeField] float maxVelocity = 5f;

    ControllerID myID;
    PlayerInput playerInput;
    Rigidbody2D m_Rigidbody;
    // TODO 
    //private Animator anim;
    //private AudioClip shootSound;

    [SerializeField] private bool b_canMove;
    public float m_moveDelay = 0.5f;

    void Awake() => InitVars();

    void InitVars()
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody2D>();

        if (myID == null)
            myID = GetComponent<ControllerID>();

        if (playerInput == null)
            playerInput = Object.FindObjectOfType<PlayerInput>();

        b_canMove = true;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    
    void MovePlayer()
    {
        float force = 0f;
        float velocity = Mathf.Abs(m_Rigidbody.velocity.x);

        float xMove = 0;
        if (myID.ID == 1)
            xMove = playerInput.GetPlayer1XInput();
        else if (myID.ID == 2)
            xMove = playerInput.GetPlayer2XInput();
        
        if (b_canMove)
        {
            // player wish to move left or right
            if (xMove != 0)
            {
                // if player has not reached max speed, and can still move
                // move on x Axis based on keyboard input (-1 = left, 1 = right)
                if (velocity < maxVelocity)
                    force = speed * Mathf.Sign(xMove);

                m_Rigidbody.AddForce(new Vector2(force, 0));

                // changing player x scale to face left or right
                FlipCharacter(Mathf.Sign(xMove));
            }
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
    
    public void StartShootingDelay()
    {
        // Start shooting delay
        StartCoroutine(MovementDelayCoroutine());
    }

    private IEnumerator MovementDelayCoroutine()
    {
        b_canMove = false;

        // wait the shooting delay time and then continue
        yield return new WaitForSeconds(m_moveDelay);

        b_canMove = true;
    }

    public void OnPlayerDeath()
    {
        StopCoroutine(MovementDelayCoroutine());

        m_Rigidbody.transform.position = new Vector2(-100, 0);

        gameObject.SetActive(false);
        
    }
}