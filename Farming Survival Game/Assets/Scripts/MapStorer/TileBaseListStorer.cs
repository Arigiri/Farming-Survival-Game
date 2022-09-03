using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MapStore", menuName = "New Tile List")]
public class TileBaseListStorer : ScriptableObject
{
   [System.Serializable]
   public class SerializableList<T> {
      public List<T> list;
      public void Add(T element)
      {
         list.Add(element);
      }
      public bool Contains(T element)
      {
         return list.Contains(element);
      }
      public int IndexOf(T element)
      {
         return list.IndexOf(element);
      }
      public void Clear()
      {
         list = new List<T>();
      }
   }
   public SerializableList<TileBase> TileList = new SerializableList<TileBase>();
   public void SaveTileList()
   {
      var output = JsonUtility.ToJson(TileList);
      var m_path =  Path.Combine(Application.dataPath + "/Resources/TileStorer.json");
      
      Debug.Log(File.Exists(m_path));
      if(File.Exists(m_path))
      {
         File.Delete(m_path);
      }
      
      File.WriteAllText(m_path, output);
      ScriptableObjectUtility.RefreshEditorProjectWindow();
      TileList.Clear();
   }

   public void ReadTileList(string jsonFile)
   {
      var input = JsonUtility.FromJson<SerializableList<TileBase>>(jsonFile);
      TileList = input;
      foreach(var tile in TileList.list)
      Debug.Log(tile.name);
      Debug.Log(TileList.list.Count);
   }
}