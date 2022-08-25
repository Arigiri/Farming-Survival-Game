using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    [SerializeField] private Tilemap _groundMap, _unitMap;
    [SerializeField] private int _levelIndex;

    public void SaveMap() {
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.LevelIndex = _levelIndex;
        newLevel.name = $"Level {_levelIndex}";

        newLevel.GroundTiles = GetTilesFromMap(_groundMap).ToList();
        newLevel.UnitTiles = GetTilesFromMap(_unitMap).ToList();



        ScriptableObjectUtility.SaveLevelFile(newLevel);

        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map) {
            foreach (var pos in map.cellBounds.allPositionsWithin) {
                if (map.HasTile(pos)) {
                    var levelTile = map.GetTile<LevelTile>(pos);
                    yield return new SavedTile() {
                        Position = pos,
                        Tile = levelTile
                    };
                }
            }
        }
    }

    public void ClearMap() {
        var maps = FindObjectsOfType<Tilemap>();

        foreach (var tilemap in maps) {
            tilemap.ClearAllTiles();
        }
    }

    public void LoadMap() {
        var level = Resources.Load<ScriptableLevel>($"Levels/Level {_levelIndex}");
        if (level == null) {
            Debug.LogError($"Level {_levelIndex} does not exist.");
            return;
        }

        ClearMap();

        foreach (var savedTile in level.GroundTiles) {
            switch (savedTile.Tile.Type) {
                case TileType.Grass:
                    SetTile(_groundMap, savedTile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (var savedTile in level.UnitTiles)
        {
            switch (savedTile.Tile.Type)
            {
                case TileType.Quinn:
                case TileType.Snorlax:
                    SetTile(_unitMap, savedTile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void SetTile(Tilemap map, SavedTile tile) {
            map.SetTile(tile.Position, tile.Tile);
        }
    }

    
}

#if UNITY_EDITOR

public static class ScriptableObjectUtility {
    public static void SaveLevelFile(ScriptableLevel level) {
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif



public struct Level {
    public int LevelIndex;
    public List<SavedTile> GroundTiles;
    public List<SavedTile> UnitTiles;

    public string Serialize() {
        var builder = new StringBuilder();

        builder.Append("g[");
        foreach (var groundTile in GroundTiles) {
            builder.Append($"{(int) groundTile.Tile.Type}({groundTile.Position.x},{groundTile.Position.y})");
        }
        builder.Append("]");

        return builder.ToString();
    }
}