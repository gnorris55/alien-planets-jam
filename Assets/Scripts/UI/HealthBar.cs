using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {

        PlanetObject planetObject = RocketShip.Instance as PlanetObject;
        if (planetObject == null )
        {
            print("rocket ship not present is null");
            gameObject.SetActive(false);
        }  
        else
        {
            planetObject.OnHealthUpdated += RocketShip_OnHealthUpdated;

        }

    }

    private void RocketShip_OnHealthUpdated(object sender, System.EventArgs e)
    {
        float currentHealth = RocketShip.Instance.GetCurrentHealth();
        float maxHealth = RocketShip.Instance.GetMaxHealth();
        healthFill.fillAmount = currentHealth / maxHealth;
    }
}
