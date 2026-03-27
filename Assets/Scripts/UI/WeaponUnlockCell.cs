using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UpgradeCellUI;

public class WeaponUnlockCell : MonoBehaviour
{
    public event EventHandler OnPurchaseWeapon;

    [SerializeField] private PlayerWeapon.WeaponType weaponType;
    [SerializeField] private Button unlockButton;

    [SerializeField] private TextMeshProUGUI oilCostText;
    [SerializeField] private TextMeshProUGUI blueMineralCostText;
    [SerializeField] private TextMeshProUGUI yellowMineralCostText;
    [SerializeField] private TextMeshProUGUI redMineralCostText;

    
    
    [SerializeField] private float oilCost;
    [SerializeField] private float blueMineralCost;
    [SerializeField] private float yellowMineralCost;
    [SerializeField] private float redMineralCost;







    private void Start()
    {

        if (blueMineralCost == 0)
        {
            blueMineralCostText.transform.parent.gameObject.SetActive(false);
        }
        if (yellowMineralCost == 0)
        {
            yellowMineralCostText.transform.parent.gameObject.SetActive(false);
        }
        if (redMineralCost == 0)
        {
            redMineralCostText.transform.parent.gameObject.SetActive(false);
        }


        oilCostText.text = oilCost.ToString();
        blueMineralCostText.text = blueMineralCost.ToString();
        yellowMineralCostText.text = yellowMineralCost.ToString();
        redMineralCostText.text = redMineralCost.ToString();

        unlockButton.onClick.AddListener(UnlockWeapon);


    }

    public PlayerWeapon.WeaponType GetWeaponType()
    {
        return weaponType;
    }

    private void UnlockWeapon()
    {
        if (Player.Instance.BuyUpgrade(oilCost/100f, blueMineralCost/10f, yellowMineralCost/10f, redMineralCost/10f))
        {
            PlayerWeapon.Instance.UnlockWeapon(weaponType);
            OnPurchaseWeapon?.Invoke(this, EventArgs.Empty);
        }
    }


}
