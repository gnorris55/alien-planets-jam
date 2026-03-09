using UnityEngine;

public class PlanetObject: MonoBehaviour
{

    [SerializeField] private float width;


    public virtual void Interact(Player player)
    {
        print("player");

    }

    public float GetWidth()
    {
        return width;
    }
}
