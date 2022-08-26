using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableRecipe : ScriptableObject
{
    public List<CollectableObjectController> Material;
    public CollectableObjectController Result;
    public string Name;
}
