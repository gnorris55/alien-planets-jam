using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "PlanetStrcuturesSO", menuName = "Scriptable Objects/PlanetStrcuturesSO")]
public class PlanetStructuresSO : ScriptableObject
{
    public List<PlanetStructureSO> planetStructures;
}
