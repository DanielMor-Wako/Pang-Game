using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    // init player vars
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