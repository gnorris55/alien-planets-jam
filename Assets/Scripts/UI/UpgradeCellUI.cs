using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCellUI : MonoBehaviour
{
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectNamePlusUpgradeType;

    public void SetUp(UpgradeRequirementsSO upgradeRequirementsSO)
    {
        objectImage.sprite = upgradeRequirementsSO.objectTextureSprite;
        objectNamePlusUpgradeType.text = upgradeRequirementsSO.objectName;
    }
}
