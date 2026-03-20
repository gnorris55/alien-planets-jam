using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeRequirementsSO", menuName = "Scriptable Objects/UpgradeRequirementsSO")]
public class UpgradeRequirementsSO : ScriptableObject
{
    public StatsManager.ObjectType type;
    
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

    public UpgradeRequirements upgradeRequirements;

    public UpgradeValuesSO upgradeValues;
    

}

