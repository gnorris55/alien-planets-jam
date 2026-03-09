using UnityEngine;

[CreateAssetMenu(fileName = "PlanetStructure", menuName = "Scriptable Objects/PlanetStructure")]
public class PlanetStructureSO : ScriptableObject
{
    public string structureName;
    public PlanetObject structureGameObject;
    public GameObject structureOutline;
    public float size;
}
