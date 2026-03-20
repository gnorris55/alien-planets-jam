using UnityEngine;

public class RocketShip : PlanetObject
{

    protected override void Start()
    {
        base.Start();
        StatsManager.Instance.GetGameObjectStats(StatsManager.ObjectType.rocketShip);
        SetHealth(GetMaxHealth());
    }

    protected override void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        if (e.objectType == StatsManager.ObjectType.rocketShip)
        {

            print("current level: " + e.currentLevel);
            if (e.currentLevel == 1)
            {
                SetInteractable();
            }
            

        }
    }
    public override void Interact(Player player)
    {
        Debug.Log("Player has one the game");
    }



    private void SetInteractable()
    {
        isInteractable = true;
    }


    public override void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("PRESS E TO GO HOME");
    }

    public override void TakeDamage(float damageAmount)
    {
        DamageStructure(damageAmount);
        if (IsDestroyed())
        {

            Debug.Log("Player has lost the game");
            DestoryPlanetObject();
        }
    }


}
