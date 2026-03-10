using DG.Tweening;
using UnityEngine;

public class PlanetObject: MonoBehaviour
{

    [SerializeField] private float width;


    protected bool isInteractable = true;

    private void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f));
        mySequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f));
    }

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
