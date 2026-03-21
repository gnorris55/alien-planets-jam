using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class UpgradeCellUI : MonoBehaviour
{

    public event EventHandler <OnUpgradeArgs>OnUpgrade;
    public class OnUpgradeArgs : EventArgs
    {
        public UpgradeRequirementsSO upgradeRequirementsSO;
    };

    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectNamePlusUpgradeType;
    [SerializeField] private TextMeshProUGUI levelDisplayText;
    [SerializeField] private Button upgradeButton;


    [SerializeField] private Transform upgradeEffectsContainerTransform;
    
    [SerializeField] private TextMeshProUGUI oilAmountRequiredText;
    [SerializeField] private TextMeshProUGUI blueMineralAmountRequiredText;
    [SerializeField] private TextMeshProUGUI yellowMineralAmountRequiredText;
    [SerializeField] private TextMeshProUGUI redMineralAmountRequiredText;


    private UpgradeRequirementsSO upgradeRequirementsSO;

    private void Awake()
    {
        blueMineralAmountRequiredText.transform.parent.gameObject.SetActive(false);
        yellowMineralAmountRequiredText.transform.parent.gameObject.SetActive(false);
        redMineralAmountRequiredText.transform.parent.gameObject.SetActive(false);
    }

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeButtonPressed);
    }


    private void UpgradeButtonPressed()
    {
        int currentLevel = StatsManager.Instance.GetCurrentLevel(upgradeRequirementsSO.type);
        float requiredOilAmount = upgradeRequirementsSO.upgradeRequirements.oilAmount.Evaluate(currentLevel);
        float requiredBlueMineralAmount = upgradeRequirementsSO.upgradeRequirements.blueMineralAmount.Evaluate(currentLevel);
        float requiredYellowMineralAmount = upgradeRequirementsSO.upgradeRequirements.yellowMineralAmount.Evaluate(currentLevel);
        float requiredRedMineralAmount = upgradeRequirementsSO.upgradeRequirements.redMineralAmount.Evaluate(currentLevel);

        if (Player.Instance.BuyUpgrade(requiredOilAmount, requiredBlueMineralAmount, requiredYellowMineralAmount, requiredRedMineralAmount))
        {
            OnUpgrade?.Invoke(this, new OnUpgradeArgs
            {
                upgradeRequirementsSO = upgradeRequirementsSO
            });
        }
    }

    public void SetUp(UpgradeRequirementsSO upgradeRequirementsSO)
    {
        int currentLevel = StatsManager.Instance.GetCurrentLevel(upgradeRequirementsSO.type);

        this.upgradeRequirementsSO = upgradeRequirementsSO;
        objectImage.sprite = upgradeRequirementsSO.objectTextureSprite;
        objectNamePlusUpgradeType.text = upgradeRequirementsSO.objectName;
        levelDisplayText.text = currentLevel + " -> " + (currentLevel + 1);

        UpgradeValuesSO upgradeValuesSO = upgradeRequirementsSO.upgradeValues;

        DisplayUpgradeValuesInformation("Health: ", upgradeValuesSO.healthUpgradeValues, currentLevel);
        DisplayUpgradeValuesInformation("Storage: ", upgradeValuesSO.storageUpgradeValues, currentLevel);
        DisplayUpgradeValuesInformation("Damage: ", upgradeValuesSO.damageUpgradeValues, currentLevel);

        oilAmountRequiredText.text = (upgradeRequirementsSO.upgradeRequirements.oilAmount.Evaluate(currentLevel) * 100).ToString();

        DisplayUpgradeCostInformation(ref blueMineralAmountRequiredText, upgradeRequirementsSO.upgradeRequirements.blueMineralAmount, currentLevel, 10f);
        DisplayUpgradeCostInformation(ref yellowMineralAmountRequiredText, upgradeRequirementsSO.upgradeRequirements.yellowMineralAmount, currentLevel, 10f);
        DisplayUpgradeCostInformation(ref redMineralAmountRequiredText, upgradeRequirementsSO.upgradeRequirements.redMineralAmount, currentLevel, 10f);
    }

    private void DisplayUpgradeCostInformation(ref TextMeshProUGUI upgradeCostText, AnimationCurve upgradeCostCurve, int currentLevel, float costScalar)
    {
        float upgradeCost = (upgradeCostCurve.Evaluate(currentLevel) * costScalar);
        if (upgradeCost > 0) 
        {
            upgradeCostText.transform.parent.gameObject.SetActive(true);
            upgradeCostText.text = upgradeCost.ToString();
        }

    }

    private void DisplayUpgradeValuesInformation(String upgradeTypeString, AnimationCurve upgradeValues, int currentLevel)
    {
        if (upgradeValues.Evaluate(currentLevel) > 0)
        {
            int currentLevelValue = (int)(upgradeValues.Evaluate(currentLevel) * 100f);
            int nextLevelValue = (int)(upgradeValues.Evaluate(currentLevel+1) * 100f);
            int valueDifference = nextLevelValue - currentLevelValue;

            TextMeshProUGUI upgradeValueEffectText = new TextMeshProUGUI();
            upgradeValueEffectText.text = upgradeTypeString + currentLevelValue.ToString() + " (+" + valueDifference + ")";

            //Instantiate(upgradeValueEffectText, upgradeEffectsContainerTransform);
            
        }

    }

}
