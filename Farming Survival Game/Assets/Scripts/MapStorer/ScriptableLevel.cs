using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScriptableLevel : ScriptableObject {
    public int LevelIndex;
    public List<SavedTile> TileMap;
}

[Serializable]
public class SavedTile {
    public Vector3Int Position;
    public TileBase m_Tile;
}