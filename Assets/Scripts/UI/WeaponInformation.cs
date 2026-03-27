using UnityEngine;

public class WeaponInformation : MonoBehaviour
{
    [SerializeField] private Transform pistolInformationBox;
    [SerializeField] private Transform shotGunInformationBox;
    [SerializeField] private Transform machineGunInformationBox;



    private void Start()
    {
        pistolInformationBox.gameObject.SetActive(false);
        shotGunInformationBox.gameObject.SetActive(false);
        machineGunInformationBox.gameObject.SetActive(false);



        PlayerWeapon.Instance.OnWeaponUnlocked += PlayerWeapon_OnWeaponUnlocked;
    }

    private void PlayerWeapon_OnWeaponUnlocked(object sender, PlayerWeapon.WeaponType weaponType)
    {

        if (weaponType == PlayerWeapon.WeaponType.pistol)
        {
            pistolInformationBox.gameObject.SetActive(true);
        }

        else if (weaponType == PlayerWeapon.WeaponType.shotgun)
        {
            shotGunInformationBox.gameObject.SetActive(true);
        }
        else if (weaponType == PlayerWeapon.WeaponType.machinegun) 
        {
            machineGunInformationBox.gameObject.SetActive(true);
        }
    }
}
