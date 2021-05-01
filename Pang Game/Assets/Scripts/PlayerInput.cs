using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // init player vars
    [Header("Player 1")]
    [SerializeField] Vector2 XYMovement1;
    private Vector2 XYTouchMovement1;
    [Header("Player 2")]
    [SerializeField] Vector2 XYMovement2;
    private Vector2 XYTouchMovement2;
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
        float xMove;
        float yMove;

        // update player 1 input
        // check for touch input, 
        if (XYTouchMovement1 != Vector2.zero)
        {
            xMove = XYTouchMovement1.x;
            yMove = XYTouchMovement1.y;
        } else
        {
            // otherwise keyboard
            xMove = Input.GetAxisRaw("Horizontal");
            yMove = Input.GetAxisRaw("Vertical");
        }
        newInput = new Vector2(xMove, yMove);
        XYMovement1 = newInput;
        
        // update player 2 input
        // check for touch input, 
        if (XYTouchMovement2 != Vector2.zero)
        {
            xMove = XYTouchMovement2.x;
            yMove = XYTouchMovement2.y;
        } else
        {
            xMove = Input.GetAxisRaw("Horizontal2");
            yMove = Input.GetAxisRaw("Vertical2");
        }
        newInput = new Vector2(xMove, yMove);
        XYMovement2 = newInput;
    }

    // allow players to move by ui_buttons
    public void SetPlayer1xDir(int xDir)
    {
        XYTouchMovement1.x = xDir;
    }
    public void SetPlayer1yDir(int yDir)
    {
        XYTouchMovement1.y = yDir;
    }
    public void SetPlayer2xDir(int xDir)
    {
        XYTouchMovement2.x = xDir;
    }
    public void SetPlayer2yDir(int yDir)
    {
        XYTouchMovement2.y = yDir;
    }

    // returns the current input value
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
