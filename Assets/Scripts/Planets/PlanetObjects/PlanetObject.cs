using UnityEngine;

public class PlanetObject: MonoBehaviour
{

    [SerializeField] private float width;


    protected bool isInteractable = true;
    public virtual void Interact(Player player)
    {

    }
    public virtual void InteractStopped()
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

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public float GetWidth()
    {
        return width;
    }
}
