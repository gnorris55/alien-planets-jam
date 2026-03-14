using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeRequirementsListSO", menuName = "Scriptable Objects/UpgradeRequirementsListSO")]
public class UpgradeRequirementsListSO : ScriptableObject
{
    public List<UpgradeRequirementsSO> upgradeRequirements;
}
