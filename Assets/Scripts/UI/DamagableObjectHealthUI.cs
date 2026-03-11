using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamagableObjectHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject damagableGameObject;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private GameObject healthTextContainer;


    private IDamagable damagableGameObjectInterface;
    private TextMeshPro healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!damagableGameObject.TryGetComponent<IDamagable>(out damagableGameObjectInterface)) 
        {
            Debug.LogError("game object does not contain damagableInterface");
        }
        if (!healthTextContainer.TryGetComponent<TextMeshPro>(out healthText))
        {
            Debug.LogError("game object does not contain text mesh pro component");
        }

        UpdateHealth(damagableGameObjectInterface.GetMaxHealth(), damagableGameObjectInterface.GetMaxHealth());
        damagableGameObjectInterface.OnHealthUpdated += DamagableGameObjectInterface_OnHealthUpdated;
    }

    private void DamagableGameObjectInterface_OnHealthUpdated(object sender, System.EventArgs e)
    {
        float currentHealth = damagableGameObjectInterface.GetCurrentHealth();
        float maxHealth = damagableGameObjectInterface.GetMaxHealth();
        UpdateHealth(currentHealth, maxHealth);
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        healthText.text = (int)currentHealth + "/" + (int)maxHealth;

    }

}
