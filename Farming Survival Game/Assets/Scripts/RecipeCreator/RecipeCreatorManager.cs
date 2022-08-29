using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeCreatorManager : MonoBehaviour
{
    [SerializeField] private List<CollectableObjectController> Material;
    [SerializeField] private CollectableObjectController Result;
    [SerializeField] private string ThisRecipeName;
    public void CreateRecipe()
    {
        var newLevel = ScriptableObject.CreateInstance<ScriptableRecipe>();
        newLevel.Name = ThisRecipeName;

        newLevel.Material = Material;
        newLevel.Result = Result;
        ScriptableObjectUtility.SaveRecipeFile(newLevel);
    }
}

