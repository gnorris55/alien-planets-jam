using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RocketShip : PlanetObject
{


    public static RocketShip Instance;


    [SerializeField] private Sprite fixedRocketShipTexture;
    [SerializeField] private SpriteRenderer rocketShipSprite;
    [SerializeField] private GameObject rocketShipFireParticlesGameObject;


    private bool gameWon = false;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        StatsManager.Instance.GetGameObjectStats(StatsManager.ObjectType.rocketShip);
    }

    protected override void StatsManager_OnGameObjectStatsUpdated(object sender, StatsManager.OnGameObjectStatsUpgradedArgs e)
    {
        if (e.objectType == StatsManager.ObjectType.rocketShip)
        {
            print(e.currentLevel);
            if (e.currentLevel == 1)
            {
                SetInteractable();
                rocketShipSprite.sprite = fixedRocketShipTexture;
                rocketShipFireParticlesGameObject.SetActive(false);
            }
            float updatedMaxHealthAmount = e.upgradeValues.healthUpgradeValues.Evaluate(e.currentLevel) * 100f;
            SetMaxHealth(updatedMaxHealthAmount);
            SetHealth(GetMaxHealth());

            statsSet = true;
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
