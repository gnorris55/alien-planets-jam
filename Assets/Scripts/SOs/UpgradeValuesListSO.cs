using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeValuesListSO", menuName = "Scriptable Objects/UpgradeValuesListSO")]
public class UpgradeValuesListSO : ScriptableObject
{
    public List<UpgradeValuesSO> upgradeValuesSO; 
}
