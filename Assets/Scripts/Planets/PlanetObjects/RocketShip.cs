using UnityEngine;

public class RocketShip : PlanetObject
{

    private bool gameWon = false;
    protected override void Start()
    {
        base.Start();
        StatsManager.Instance.GetGameObjectStats(StatsManager.ObjectType.rocketShip);
        SetHealth(GetMaxHealth());
    }

    protected override void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        print(e.objectType);
        if (e.objectType == StatsManager.ObjectType.rocketShip)
        {

            /*
            float updatedMaxHealthAmount = e.upgradeValues.healthUpgradeValues.Evaluate(e.currentLevel) * 100f;
            print(updatedMaxHealthAmount);
            SetMaxHealth(updatedMaxHealthAmount);
            */

            if (e.currentLevel == 1)
            {
                SetInteractable();
            }
        }
    }
    public override void Interact(Player player)
    {
        gameWon = true;
        GameManager.Instance.PlayerWonGame();
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
        if (!gameWon)
        {
            DamageStructure(damageAmount);
            if (IsDestroyed())
            {

                GameManager.Instance.PlayerLostGame();
                DestoryPlanetObject();
            }
        }
    }


}
