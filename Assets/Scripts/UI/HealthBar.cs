using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.Instance.OnHealthUpdated += Player_OnHealthUpdated;
    }

    private void Player_OnHealthUpdated(object sender, Player.OnHealthUpdatedArgs e)
    {
        healthFill.fillAmount = e.updatedHealth / e.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
