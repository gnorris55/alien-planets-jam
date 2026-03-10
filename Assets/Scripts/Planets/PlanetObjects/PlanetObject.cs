using UnityEngine;

public class PlanetObject: MonoBehaviour
{

    [SerializeField] private float width;
       
    public virtual void Interact(Player player)
    {

    }
    public virtual void InteractStopped(Player player)
    {

    }

    public virtual void ShowInteractable()
    {

        PlayerHints.Instance.DisplayHint("PRESS E TO INTERACT");
    }
    public virtual void HideInteractable()
    {
        PlayerHints.Instance.HideHint();
    }




    public float GetWidth()
    {
        return width;
    }
}
