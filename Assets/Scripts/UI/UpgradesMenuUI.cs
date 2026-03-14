using UnityEngine;

public class UpgradesMenuUI : MonoBehaviour
{

    [SerializeField] private Transform upgradeContentsTransform;
    [SerializeField] private UpgradeCellUI upgradeCellUI;
    [SerializeField] private UpgradeRequirementsListSO upgradeRequirementsListSO;

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
        }

        foreach(UpgradeRequirementsSO upgradeRequirements in upgradeRequirementsListSO.upgradeRequirements)
        {
            UpgradeCellUI upgradeCellUIInstance = Instantiate(upgradeCellUI, upgradeContentsTransform);
            upgradeCellUIInstance.SetUp(upgradeRequirements);
        }

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
