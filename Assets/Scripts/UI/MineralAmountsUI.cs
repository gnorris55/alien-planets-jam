using TMPro;
using UnityEngine;

public class MineralAmountsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blueMineralAmountText;
    [SerializeField] private TextMeshProUGUI yellowMineralAmountText;
    [SerializeField] private TextMeshProUGUI redMineralAmountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.Instance.OnMineralAmountUpdated += Player_OnMineralAmountUpdated;
    }

    private void Player_OnMineralAmountUpdated(object sender, Player.OnMineralAmountUpdatedArgs e)
    {
        if (e.mineralType == MineralDeposit.MineralType.blue)
        {
            UpdateMineralText(ref blueMineralAmountText, e.updatedMineralAmount, e.maxMineralAmount);
        }
        else if (e.mineralType == MineralDeposit.MineralType.yellow)
        {
            UpdateMineralText(ref yellowMineralAmountText, e.updatedMineralAmount, e.maxMineralAmount);
        }
        else if (e.mineralType == MineralDeposit.MineralType.red)
        {
            UpdateMineralText(ref redMineralAmountText, e.updatedMineralAmount, e.maxMineralAmount);
        }
    }

    private void UpdateMineralText(ref TextMeshProUGUI mineralAmountText, float updatedMineralAmount, float maxMineralAmount) 
    {
        mineralAmountText.text = (int)updatedMineralAmount + "/" + (int)maxMineralAmount;
    }

}
