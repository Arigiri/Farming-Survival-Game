using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class CollectableObjectInformation : ScriptableObject
{
    public string ItemName;
    public int ItemFood;
    public int ItemStamina;
    public int ToolDurability;
    public int StartDurability;
    public CollectableType Type;
    public int Stack;
    public Action Action;
    public Sprite Icon;
    public bool IsTool;
    public TreeType m_TreeType;
    public string Description;
}
