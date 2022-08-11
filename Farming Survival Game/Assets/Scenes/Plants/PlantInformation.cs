using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Plant", menuName = "New Plant")]
public class PlantInformation : ScriptableObject
{
    public string Name;
    public int FoodProvide;
    public int StaminaProvide;
    public float DayToGrow;//In game
    public AnimatedTile PlantTile;
    public TreeType Type;
    public CollectableObjectController[] ItemsDropper;
}
