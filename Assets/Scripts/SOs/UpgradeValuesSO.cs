using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeValuesSO", menuName = "Scriptable Objects/UpgradeValuesSO")]
public class UpgradeValuesSO : ScriptableObject
{
    
    public StatsManager.ObjectType type;
    // Always 1 bigger than lists above cause we include starting values
    public AnimationCurve healthUpgradeValues;
    public AnimationCurve damageUpgradeValues;
    public AnimationCurve storageUpgradeValues;
}
