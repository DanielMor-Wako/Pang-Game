using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // init player vars
    [SerializeField] Vector2 XYMovement;

    [SerializeField] Rigidbody2D m_Rigidbody;

    private bool b_canMove, b_canShoot;

    // Start is called before the first frame update
    void Start()
    {
        InitVars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitVars()
    {
        b_canMove = true;
        b_canShoot = true;
    }
}
