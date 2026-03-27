using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMenuUI : MonoBehaviour
{

    [SerializeField] private Transform upgradeContentsTransform;
    [SerializeField] private UpgradeCellUI upgradeCellUI;
    [SerializeField] private UpgradeRequirementsListSO upgradeRequirementsListSO;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private GameObject shotGunUnlockCell;
    [SerializeField] private GameObject machineGunUnlockCell;

    private void Start()
    {
        CreateUpgrades();

    }

    private void CreateUpgrades()
    {
        // clear contents
        foreach (Transform child in upgradeContentsTransform)
        {
            GameObject.Destroy(child.gameObject);
            if (child.gameObject.TryGetComponent(out UpgradeCellUI upgradeCellUI))
            {
                upgradeCellUI.OnUpgrade -= UpgradeCellUIInstance_OnUpgrade;
            }
        }

        foreach(UpgradeRequirementsSO upgradeRequirementsSO in upgradeRequirementsListSO.upgradeRequirements)
        {
            if (!StatsManager.Instance.IsMaxLevel(upgradeRequirementsSO.type))
            {
                UpgradeCellUI upgradeCellUIInstance = Instantiate(upgradeCellUI, upgradeContentsTransform);
                upgradeCellUIInstance.SetUp(upgradeRequirementsSO);
                upgradeCellUIInstance.OnUpgrade += UpgradeCellUIInstance_OnUpgrade;
            }
        }
        
        WeaponUnlockCell shotGunUnlockCellInstance = Instantiate(shotGunUnlockCell, upgradeContentsTransform).GetComponent<WeaponUnlockCell>();
        shotGunUnlockCellInstance.OnPurchaseWeapon += ShotGunUnlockCellInstance_OnPurchaseWeapon1; 
        
        if (PlayerWeapon.Instance.WeaponIsUnlocked(shotGunUnlockCellInstance.GetWeaponType()))
        {
            Destroy(shotGunUnlockCellInstance.gameObject);
        }

        WeaponUnlockCell machineGunUnlockCellInstance = Instantiate(machineGunUnlockCell, upgradeContentsTransform).GetComponent<WeaponUnlockCell>();
        machineGunUnlockCellInstance.OnPurchaseWeapon += MachineGunUnlockCellInstance_OnPurchaseWeapon1;

        if (PlayerWeapon.Instance.WeaponIsUnlocked(machineGunUnlockCellInstance.GetWeaponType()))
        {
            Destroy(machineGunUnlockCellInstance.gameObject);
        }
        

       
    }

    private void MachineGunUnlockCellInstance_OnPurchaseWeapon1(object sender, System.EventArgs e)
    {
        CreateUpgrades();
    }

    private void ShotGunUnlockCellInstance_OnPurchaseWeapon1(object sender, System.EventArgs e)
    {
        CreateUpgrades();
    }


    private void UpgradeCellUIInstance_OnUpgrade(object sender, UpgradeCellUI.OnUpgradeArgs e)
    {
        // TODO check if the player has enough resources

        //if (e.upgradeRequirementsSO.upgradeRequirements.oilAmount < player

        StatsManager.Instance.UpgradeStats(e.upgradeRequirementsSO.type);
        CreateUpgrades();

    }

    public void Show()
    {
        gameObject.SetActive(true);
        scrollbar.value = 1;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
