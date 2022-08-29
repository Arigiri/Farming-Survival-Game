using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUnitController : MonoBehaviour
{
    [SerializeField] private RecipeUnit[] UnitList;
    public void ShowCraftingUnit(int Index)
    {
        foreach(var Unit in UnitList)
        {
            Unit.gameObject.SetActive(Unit.UnitIndex == Index);
        }
    }
}
