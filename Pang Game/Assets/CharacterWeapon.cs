using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    ControllerID myID;
    PlayerInput playerInput;
    CharacterController2D controller;

    public enum WeaponPrefab { Rope, Shot, StickyRope, Laser }
    public WeaponPrefab activeWeapon;
    // storing a reference to the current weapon pool, to check for available spots before spawning
    private ObjectPool activeWeaponPool;

    // max active shots that can exist simultaionasly on the scene
    [Range(1, 5)] [SerializeField] private int maxActiveShots;
    // max duration of sticky weapons (that sticks to the ceiling), 0 = none;
    [Range(0, 5)] [SerializeField] private int maxStickTime;

    [SerializeField] private bool b_canShoot;
    public float m_shootDelay = 0.5f;

    void Awake() => InitVars();

    void InitVars()
    {
        if (myID == null)
            myID = GetComponent<ControllerID>();

        if (playerInput == null)
            playerInput = Object.FindObjectOfType<PlayerInput>();

        if (controller == null)
            controller = GetComponent<CharacterController2D>();

        b_canShoot = true;
    }

    void FixedUpdate()
    {
        float yMove = 0;

        if (myID.ID == 1)
            yMove = playerInput.GetPlayer1YInput();
        else if (myID.ID == 2)
            yMove = playerInput.GetPlayer2YInput();
        
        if (yMove > 0)
            ShootWeapon();
    }

    bool IsShootingAllowed()
    {
        bool result = true;

        if (!b_canShoot)
            result = false;

        // searching for our weapon prefab based on the player ID and active weapon. example "Player2Shot" "Player1Laser"
        if (activeWeaponPool == null)
            activeWeaponPool = ObjectPoolList._instance.GetRelevantPool("Player" + myID.ID + activeWeapon.ToString());

        if (activeWeaponPool != null)
        {
            // checking if any new slot for a shot is available (-1 = none, 0 = it has 1 in the array)
            int activeShots = activeWeaponPool.ReturnActiveObjectsCount();
            int avaialbleSlots = maxActiveShots - activeShots;
            Debug.Log("NewAvaialbleSlots " + avaialbleSlots);
            if (avaialbleSlots <= 0)
                result = false;
        }

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
        string weaponName = "Player" + myID.ID + activeWeapon.ToString();
        ObjectPoolList._instance.SpawnObject(weaponName, transform.position);
        //ObjectPoolList._instance.SpawnObject(activeWeapon.ToString(), transform.position); 

        // wait the shooting delay time and then continue
        yield return new WaitForSeconds(m_shootDelay);

        b_canShoot = true;
    }
}
