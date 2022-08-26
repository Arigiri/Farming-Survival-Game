using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RecipeCreatorManager))]
public class RecipeCreatorEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var script = (RecipeCreatorManager) target;

        if(GUILayout.Button("Create Recipe"))
        {
            script.CreateRecipe();
        }

    }
}
