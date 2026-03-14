using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MineralProgressBar : MonoBehaviour
{
    [SerializeField] private Image mineralProgressFill;
    [SerializeField] private MineralDeposit mineralDeposit;
    [SerializeField] private GameObject mineralAmountTextGameObject;


    private TextMeshPro mineralAmountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mineralAmountText = mineralAmountTextGameObject.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        mineralProgressFill.fillAmount = mineralDeposit.GetMineralAmount() / mineralDeposit.GetMaxMineralAmount();
        mineralAmountText.text = (int)mineralDeposit.GetMineralAmount() + "/" + (int)mineralDeposit.GetMaxMineralAmount();
    }
}
