using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // init player vars
    [SerializeField] Vector2 XYMovement;
    private Vector2 newInput;
    
    void Awake() => InitVars();

    void InitVars()
    {
        XYMovement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        Vector2 prevInput = newInput;
        newInput = new Vector2(xMove, yMove);

        XYMovement = newInput;
    }

    public float GetPlayerXInput()
    {
        return XYMovement.x;
    }
    public float GetPlayerYInput()
    {
        return XYMovement.y;
    }
}
