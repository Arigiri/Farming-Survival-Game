using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUnitManager : MonoBehaviour
{
    [SerializeField] private int UnitIndex;

    public void ShowCraftingUnit()
    {
        var UnitList = GameObject.FindGameObjectWithTag("ToolRecipe").GetComponent<RecipeUnitController>();
        UnitList.ShowCraftingUnit(UnitIndex);
    }
}
