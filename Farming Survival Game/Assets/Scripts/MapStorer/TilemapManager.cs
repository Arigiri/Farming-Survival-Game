using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    [SerializeField] private Tilemap[] _TileMapsToSave;
    [SerializeField] private ScriptableLevel _Level;
    [SerializeField] private Tilemap _TileMapToLoad;
    private List<TileBase> TileList = new List<TileBase>();

    public void SaveMap() {
        
        foreach(var tilemap in _TileMapsToSave)
        {
            var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();
            newLevel.name = tilemap.name;

            newLevel.TileMap = GetTilesFromMap(tilemap).ToList();
            ScriptableObjectUtility.SaveLevelFile(newLevel);
        }
       



        

        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map) {
            foreach (var pos in map.cellBounds.allPositionsWithin) {
                if (map.HasTile(pos)) {
                    var levelTile = map.GetTile(pos);
                    if(TileList.Contains(levelTile) == false)TileList.Add(levelTile);
                    yield return new SavedTile() {
                        Position = pos,
                        TileIndex = TileList.IndexOf(levelTile)
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
        var level = _Level;//Resources.Load<ScriptableLevel>($"Assets/Resources/Levels/Level {_levelIndex}");
        if (level == null) {
            Debug.LogError($"Tilemap does not exist.");
            return;
        }

        ClearMap();

        foreach (var savedTile in level.TileMap) {
           _TileMapToLoad.SetTile(savedTile.Position, TileList[savedTile.TileIndex]);
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
    
    public static void SaveRecipeFile(ScriptableRecipe recipe)
    {
        AssetDatabase.CreateAsset(recipe, $"Assets/Recipes/{recipe.Name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif


