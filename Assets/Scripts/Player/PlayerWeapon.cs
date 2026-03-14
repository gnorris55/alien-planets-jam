using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

public class PlayerWeapon : MonoBehaviour
{

    private List<bool> weaponIsUnlockedList;

    public enum WeaponType
    {
        pistol,
        shotgun,
        machinegun
    }

    [SerializeField] private Transform testTriangle;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnTransform;
    [SerializeField] private float playerDamage;


    private WeaponType currentWeapon = WeaponType.pistol;
    private bool isShooting = false;
    private bool inCombat = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameInput.Instance.OnShootInputPressed += GameInput_OnShootInputPressed;
        GameInput.Instance.OnShootInputReleased += GameInput_OnShootInputReleased;
        GameInput.Instance.OnSwapWeaponInputPressed += GameInput_OnSwapWeaponInputPressed;

        Player.Instance.OnPlayerStateChanged += Player_OnPlayerStateChanged;

        SetUpWeaponUnlockList();

        // for testing

        UnlockWeapon(WeaponType.shotgun);
        UnlockWeapon(WeaponType.machinegun);

        currentWeapon = WeaponType.pistol;

    }

    private void Player_OnPlayerStateChanged(object sender, Player.OnPlayerStateChangedArgs e)
    {
        if (e.playerState == Player.PlayerStates.combat)
        {
            inCombat = true;
        }
        else
        {
            inCombat = false;
        }
    }

    private void GameInput_OnSwapWeaponInputPressed(object sender, EventArgs e)
    {
        if (inCombat)
        {


            int weaponIndex = ((int)currentWeapon + 1) % weaponIsUnlockedList.Count;
            while (true)
            {
                if (weaponIsUnlockedList[weaponIndex])
                {
                    currentWeapon = (WeaponType)weaponIndex;
                    break;
                }
                weaponIndex = (weaponIndex + 1) % weaponIsUnlockedList.Count;
            }

            Debug.Log(currentWeapon);
        }
    }

    private void GameInput_OnShootInputReleased(object sender, EventArgs e)
    {
        isShooting = false;
    }


    private void GameInput_OnShootInputPressed(object sender, System.EventArgs e)
    {
        if (inCombat)
        {


            isShooting = true;

            if (currentWeapon == WeaponType.pistol)
            {
                Vector3 shootDirection = (bulletSpawnTransform.position - transform.position).normalized;
                SpawnBullet(shootDirection, playerDamage, 6f);

                CameraManager.Instance.ShakeCamera(1f, 0.1f);
            }
            else if (currentWeapon == WeaponType.shotgun)
            {


                int numShotGunBullets = 5;
                float shotgunSpreadAngle = 2.5f;

                Vector3 shootDirection = (bulletSpawnTransform.position - transform.position).normalized;

                for (int i = -(numShotGunBullets / 2); i < (numShotGunBullets / 2) + 1; i++)
                {
                    float spreadAngle = shotgunSpreadAngle * ((float)i / (numShotGunBullets / 2));
                    Quaternion shotgunSpread = Quaternion.Euler(0, 0, spreadAngle);

                    Vector3 angledShootDirection = shotgunSpread * shootDirection;
                    SpawnBullet(angledShootDirection, playerDamage / 3f, 4f, 0.5f);
                }


                CameraManager.Instance.ShakeCamera(1f, 0.1f);
            }
        }


    }

    private void SpawnBullet(Vector3 shootDirection, float damageAmount, float speed, float despawnTime = 2f)
    {
            Transform bulletTransform = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.identity);
            bulletTransform.GetComponent<Bullet>().Setup(shootDirection, damageAmount, speed, despawnTime);

    }

    private void UnlockWeapon(WeaponType unlockedWeapon)
    {
        weaponIsUnlockedList[(int)unlockedWeapon] = true;
        currentWeapon = unlockedWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        transform.right = GetMouseDirectionFromPlayer();
    }

    // ---MISC---

    private int GetAmountOfWeaponsUnlocked()
    {
        int weaponCount = 0;
        foreach (bool WeaponUnlocked in weaponIsUnlockedList)
        {
            if (WeaponUnlocked)
            {
                weaponCount++;
            }
            else
            {
                break;
            }
        }

        return weaponCount;
    }
    private void SetUpWeaponUnlockList()
    {
        weaponIsUnlockedList = new List<bool>();
        int amountOfWeapons = Enum.GetNames(typeof(WeaponType)).Length;
        weaponIsUnlockedList.Add(true);
        for (int i = 1; i < amountOfWeapons; i++)
        {
            weaponIsUnlockedList.Add(false);
        }


    }
    private Vector2 GetMouseDirectionFromPlayer()
    {
        // convert mouse position into world coordinates
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        return direction;
    }
}
