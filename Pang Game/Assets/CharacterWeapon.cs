using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController2D controller;

    public enum WeaponPrefab { Rope, Shot, StickyRope, Laser }
    public WeaponPrefab activeWeapon;
    // storing a reference to the current weapon pool, to check for available spots before spawning
    private ObjectPool activeWeaponPool;

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

    bool IsShootingAllowed()
    {
        bool result = true;

        if (!b_canShoot)
            result = false;
        
        if (activeWeaponPool == null)
            activeWeaponPool = ObjectPoolList._instance.GetRelevantPool(activeWeapon.ToString());

        // checking if any new slot for a shot is available (-1 = none)
        int NewAvaialbleSlots = activeWeaponPool.ReturnCountOfAllAvailableObjects();
        //Debug.Log("NewAvaialbleSlots " + (NewAvaialbleSlots+1));
        if (NewAvaialbleSlots == -1)
            result = false;

        return result;
    }

    void ShootWeapon()
    {
        // check for current weapon requirements
        bool allowShooting = IsShootingAllowed();
        // if any restrictions apllied, then abort shooting
        if (!allowShooting)
            return;

        // Start shooting coroutine
        StartCoroutine(ShootCoroutine());

        // Call aborting movement coroutine
        controller.StartShootingDelay();
    }

    private IEnumerator ShootCoroutine()
    {
        b_canShoot = false;
        ObjectPoolList._instance.SpawnObject(activeWeapon.ToString(), transform.position);

        // wait the shooting delay time and then continue
        yield return new WaitForSeconds(m_shootDelay);

        b_canShoot = true;
    }
}
