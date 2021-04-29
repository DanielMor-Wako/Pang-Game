﻿using System.Collections;
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
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        newInput = new Vector2(xMove, yMove);
        XYMovement = newInput;
    }

    public Vector2 GetPlayerInput()
    {
        return XYMovement;
    }
}
