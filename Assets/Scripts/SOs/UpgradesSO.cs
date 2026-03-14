using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeRequirementsSO", menuName = "Scriptable Objects/UpgradeRequirementsSO")]
public class UpgradeRequirementsSO : ScriptableObject
{
    
    [System.Serializable]
    public struct UpgradeRequirements
    {
        public AnimationCurve oilAmount;
        public AnimationCurve blueMineralAmount;
        public AnimationCurve yellowMineralAmount;
        public AnimationCurve redMineralAmount;
    };

    public string objectName;
    public Sprite objectTextureSprite;

    public UpgradeRequirements healthUpgradeRequirements;
    public UpgradeRequirements damageUpgradeRequirements;
    public UpgradeRequirements storageUpgradeRequirements;
    

}

[CreateAssetMenu(fileName = "UpgradeValuesSO", menuName = "Scriptable Objects/UpgradeValuesSO")]
public class UpgradeValuesSO : ScriptableObject
{
    // Always 1 bigger than lists above cause we include starting values
    public List<float> healthUpgradeValues;
    public List<float> damageUpgradeValues;
    public List<float> storageUpgradeValues;

}

[CreateAssetMenu(fileName = "UpgradeValuesListSO", menuName = "Scriptable Objects/UpgradeValuesListSO")]
public class UpgradeValuesListSO : ScriptableObject
{
    public List<UpgradeValuesSO> upgradeValues;
}


