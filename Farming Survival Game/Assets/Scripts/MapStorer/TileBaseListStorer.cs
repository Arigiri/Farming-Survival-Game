using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapStore", menuName = "New Tile List")]
public class TileBaseListStorer : ScriptableObject
{
   public List<TileBase> TileList = new List<TileBase>();
}