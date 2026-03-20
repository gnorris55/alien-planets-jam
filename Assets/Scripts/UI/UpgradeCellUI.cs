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
    [SerializeField] private Button upgradeButton;

    [SerializeField] private TextMeshProUGUI healthUpgradeValuesText;
    [SerializeField] private TextMeshProUGUI storageUpgradeValuesText;
    [SerializeField] private TextMeshProUGUI damageUpgradeValuesText;

    [SerializeField] private TextMeshProUGUI oilAmountRequired;
    [SerializeField] private TextMeshProUGUI blueMineralAmountRequired;
    [SerializeField] private TextMeshProUGUI yellowMineralAmountRequired;
    [SerializeField] private TextMeshProUGUI redMineralAmountRequired;


    private UpgradeRequirementsSO upgradeRequirementsSO;

    private void Awake()
    {
        healthUpgradeValuesText.gameObject.SetActive(false);
        storageUpgradeValuesText.gameObject.SetActive(false);
        damageUpgradeValuesText.gameObject.SetActive(false);

        healthUpgradeValuesText.text = "Health: ";
        storageUpgradeValuesText.text = "Storage: ";
        damageUpgradeValuesText.text = "Damage: ";



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
        objectNamePlusUpgradeType.text = upgradeRequirementsSO.objectName + "(" + currentLevel + "->" + (currentLevel + 1) + ")";

        UpgradeValuesSO upgradeValuesSO = upgradeRequirementsSO.upgradeValues;
        //print(upgradeValuesSO.healthUpgradeValues.Evaluate(currentLevel));

        DisplayUpgradeValuesInformation(ref healthUpgradeValuesText, upgradeValuesSO.healthUpgradeValues, currentLevel);
        DisplayUpgradeValuesInformation(ref storageUpgradeValuesText, upgradeValuesSO.storageUpgradeValues, currentLevel);
        DisplayUpgradeValuesInformation(ref damageUpgradeValuesText, upgradeValuesSO.damageUpgradeValues, currentLevel);

        oilAmountRequired.text = (upgradeRequirementsSO.upgradeRequirements.oilAmount.Evaluate(currentLevel)*100f).ToString();
        blueMineralAmountRequired.text = (upgradeRequirementsSO.upgradeRequirements.blueMineralAmount.Evaluate(currentLevel)*10f).ToString();
        yellowMineralAmountRequired.text = (upgradeRequirementsSO.upgradeRequirements.yellowMineralAmount.Evaluate(currentLevel)*10f).ToString();
        redMineralAmountRequired.text = (upgradeRequirementsSO.upgradeRequirements.redMineralAmount.Evaluate(currentLevel)*10f).ToString();

    }

    private void DisplayUpgradeValuesInformation(ref TextMeshProUGUI upgradeValueText, AnimationCurve upgradeValues, int currentLevel)
    {
        if (upgradeValues.Evaluate(currentLevel) > 0)
        {
            upgradeValueText.gameObject.SetActive(true);
            int currentLevelValue = (int)(upgradeValues.Evaluate(currentLevel) * 100f);
            int nextLevelValue = (int)(upgradeValues.Evaluate(currentLevel+1) * 100f);
            int valueDifference = nextLevelValue - currentLevelValue;

            upgradeValueText.text += currentLevelValue.ToString() + " (+" + valueDifference + ")";
        }

    }

}
