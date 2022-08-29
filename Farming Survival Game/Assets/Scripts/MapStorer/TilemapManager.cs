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
    [SerializeField] private ScriptableLevel[] _Levels;
    [SerializeField] private Tilemap[] _TileMapsToLoad;
    private List<TileBase> TileList;
    public TileBaseListStorer TileStorer;

    public void SaveMap() {
        
        foreach(var tilemap in _TileMapsToSave)
        {
            var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();
            TileList = new List<TileBase>();
            newLevel.name = tilemap.name;

            newLevel.TileMap = GetTilesFromMap(tilemap).ToList();
            // TileStorer.TileList.AddRange(TileList);
            ScriptableObjectUtility.SaveLevelFile(newLevel);
        }
       



        

        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map) {
            foreach (var pos in map.cellBounds.allPositionsWithin) {
                if (map.HasTile(pos)) {
                    var levelTile = map.GetTile(pos);
                    if(TileStorer.TileList.Contains(levelTile) == false)TileStorer.TileList.Add(levelTile);
                    yield return new SavedTile() {
                        Position = pos,
                        TileIndex = TileStorer.TileList.IndexOf(levelTile)
                    };
                }
            }
        }
    }

    public void ClearMap() {
        var maps = _TileMapsToLoad;

        foreach (var tilemap in maps) {
            tilemap.ClearAllTiles();
        }
    }

    public void LoadMap() {
        var level = _Levels;//Resources.Load<ScriptableLevel>($"Assets/Resources/Levels/Level {_levelIndex}");
        if (level == null) {
            Debug.LogError($"Tilemap does not exist.");
            return;
        }
        ClearMap();
        foreach(var Level in level)
        {
            Tilemap m_Tilemap = null;
            foreach(var tilemap in _TileMapsToLoad)
            {
                if(tilemap.name == Level.name)
                {
                    m_Tilemap = tilemap;
                    break;
                }
            }
            if(m_Tilemap == null)continue;
            foreach(var savedTile in Level.TileMap)
            {
                m_Tilemap.SetTile(savedTile.Position, TileStorer.TileList[savedTile.TileIndex]);
            }
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

    public static void SaveTileList(TileBaseListStorer tilelist)
    {
        AssetDatabase.CreateAsset(tilelist, $"Assets/Resources/Levels/TileList.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif


