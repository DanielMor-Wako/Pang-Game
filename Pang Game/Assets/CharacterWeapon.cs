using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController2D controller;

    public enum WeaponPrefab { Rope, Shot, StickyRope, Laser }
    public WeaponPrefab activeWeapon;

    // max active shots that can exist simultaionasly on the scene
    [Range(0, 5)] [SerializeField] private int maxActiveShots;
    // max duration of sticky weapons (that sticks to the ceiling), 0 = none;
    [Range(0, 5)] [SerializeField] private int maxStickTime;

    [SerializeField] private bool b_canShoot;
    public float m_shootDelay = 0.5f;

    void Awake() => InitVars();

    void InitVars()
    {
        if (playerInput == null)
            playerInput = Object.FindObjectOfType<PlayerInput>();

        if (controller == null)
            controller = GetComponent<CharacterController2D>();

        b_canShoot = true;
    }

    void FixedUpdate()
    {
        float yMove = playerInput.GetPlayerYInput();
        if (yMove > 0)
        {
            ShootWeapon();
        }
    }

    void ShootWeapon()
    {
        // check for current weapon requirements
        if (!b_canShoot)
            return;

        // Start shooting coroutine
        StartCoroutine(ShootCoroutine());

        // Call aborting movement coroutine
        controller.StartShootingDelay();
    }

    private IEnumerator ShootCoroutine()
    {
        b_canShoot = false;
        Debug.Log("b_canShoot = " + b_canShoot);

        // wait the shooting delay time and then continue
        yield return new WaitForSeconds(m_shootDelay);

        b_canShoot = true;
        Debug.Log("b_canShoot = " + b_canShoot);
    }
}
