using UnityEngine;

public class WeaponInformation : MonoBehaviour
{
    [SerializeField] private GameObject pistolInformationBox;
    [SerializeField] private GameObject shotGunInformationBox;
    [SerializeField] private GameObject machineGunInformationBox;



    private void Start()
    {
        
        pistolInformationBox.SetActive(false);
        shotGunInformationBox.SetActive(false);
        machineGunInformationBox.SetActive(false);
       


        PlayerWeapon.Instance.OnWeaponUnlocked += PlayerWeapon_OnWeaponUnlocked;
    }

    private void PlayerWeapon_OnWeaponUnlocked(object sender, PlayerWeapon.WeaponType weaponType)
    {

        if (weaponType == PlayerWeapon.WeaponType.pistol)
        {
            pistolInformationBox.SetActive(true);
        }

        else if (weaponType == PlayerWeapon.WeaponType.shotgun)
        {
            shotGunInformationBox.SetActive(true);
        }
        else if (weaponType == PlayerWeapon.WeaponType.machinegun) 
        {
            machineGunInformationBox.SetActive(true);
        }
    }
}
