using UnityEngine;
using UnityEngine.UI;

public class UpgradesMenuUI : MonoBehaviour
{

    [SerializeField] private Transform upgradeContentsTransform;
    [SerializeField] private UpgradeCellUI upgradeCellUI;
    [SerializeField] private UpgradeRequirementsListSO upgradeRequirementsListSO;
    [SerializeField] private Scrollbar scrollbar;

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
        scrollbar.value = 1;

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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
