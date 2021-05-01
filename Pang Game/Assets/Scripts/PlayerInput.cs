using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // init player vars
    [Header("Player 1")]
    [SerializeField] Vector2 XYMovement1;
    [Header("Player 2")]
    [SerializeField] Vector2 XYMovement2;
    private Vector2 newInput;
    
    void Awake() => InitVars();

    void InitVars()
    {
        XYMovement1 = Vector2.zero;
        XYMovement2 = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // update player 1 input
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");
        
        newInput = new Vector2(xMove, yMove);

        XYMovement1 = newInput;

        // update player 2 input
        xMove = Input.GetAxisRaw("Horizontal2");
        yMove = Input.GetAxisRaw("Vertical2");
        
        newInput = new Vector2(xMove, yMove);

        XYMovement2 = newInput;
    }

    public float GetPlayer1XInput()
    {
        return XYMovement1.x;
    }
    public float GetPlayer1YInput()
    {
        return XYMovement1.y;
    }
    public float GetPlayer2XInput()
    {
        return XYMovement2.x;
    }
    public float GetPlayer2YInput()
    {
        return XYMovement2.y;
    }
}
