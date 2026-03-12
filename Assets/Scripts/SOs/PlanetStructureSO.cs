using UnityEngine;

[CreateAssetMenu(fileName = "PlanetStructure", menuName = "Scriptable Objects/PlanetStructure")]
public class PlanetStructureSO : ScriptableObject
{
    public string structureName;
    public PlanetObject structureGameObject;
    public GameObject structureOutline;
    public float width;
    public float height;
    public Sprite sprite;
}
