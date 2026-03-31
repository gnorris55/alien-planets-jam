using UnityEngine;

public class MineralDeposit : PlanetObject
{
    public enum MineralType
    {
        blue, 
        yellow,
        red
    }

    [SerializeField] private MineralType mineralType;
    [SerializeField] private float maxMineralAmount = 100.0f;
    [SerializeField] private float mineralAccumulationSpeed = 0.05f;
    [SerializeField] private float mineralHarvestSpeed = 100f;
    [SerializeField] private ItemVisualMovement mineralGlobVisual;
    [SerializeField] private AudioSource mineralHarvestingSound;

    private float currentMineralAmount = 0;
    private bool playerIsHarvestingMineral = false;
    private float mineralTransferedCount = 0;

    private void Update()
    {
        
        if (currentMineralAmount < maxMineralAmount)
        {
            float mineralAmount = Time.deltaTime * mineralAccumulationSpeed;
            AddMineral(mineralAmount);
            if (!isInteractable && (currentMineralAmount / maxMineralAmount) > 0.1)
            {
                isInteractable = true;
            }
        }
        if (playerIsHarvestingMineral && currentMineralAmount > 0)
        {
            TransferMineralToPlayer();
        }
    }

    public override void Interact(Player player)
    {
        playerIsHarvestingMineral = true;
        mineralHarvestingSound.Play();
    }

    public override void InteractStopped()
    {
        playerIsHarvestingMineral = false;
        mineralTransferedCount = 0;
        mineralHarvestingSound.Pause(); 
    }

    public override void ShowInteractable()
    {
        PlayerHints.Instance.DisplayHint("HOLD E TO MINE MINERAL");
    }


    private void TransferMineralToPlayer()
    {

        float mineralTransferAmount = Time.deltaTime * mineralHarvestSpeed;

        Player player = Player.Instance;

        float leftOverMineral = player.AddMineral(mineralTransferAmount, mineralType);
        
        currentMineralAmount = currentMineralAmount - mineralTransferAmount + leftOverMineral;

        if (leftOverMineral == 0)
        {
            DisplayMaterialTransferVisuals(mineralTransferAmount, player.transform.position);
        }

    }

    public float AddMineral(float mineralAmount)
    {
        currentMineralAmount += mineralAmount;

        float leftOverMineral = Mathf.Clamp(currentMineralAmount - maxMineralAmount, 0, maxMineralAmount);
        if (leftOverMineral > 0)
        {
            currentMineralAmount = maxMineralAmount;
        }

       
        return leftOverMineral;

    }


    private void DisplayMaterialTransferVisuals(float transferAmount, Vector3 transferTargetLocation)
    {
        mineralTransferedCount += transferAmount;
        
        if (mineralTransferedCount > 1)
        {
            ItemVisualMovement mineralGlobInstance = Instantiate(mineralGlobVisual, transform.position, Quaternion.identity);
            mineralGlobInstance.SetUp(transform.position, transferTargetLocation);
            mineralTransferedCount = 0;
        }
    }



    public float GetMineralAmount()
    {
        return currentMineralAmount;
    }

    public float GetMaxMineralAmount()
    {
        return maxMineralAmount;
    }

}
