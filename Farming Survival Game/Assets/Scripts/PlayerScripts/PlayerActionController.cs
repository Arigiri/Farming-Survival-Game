using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerActionController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerController m_Player;
    [SerializeField] private Tilemap m_Tilemap;
    private List<OnMapObjectController> ObjectOnQueue = new List<OnMapObjectController>();
    [SerializeField]private TileController m_TileController;
    [SerializeField] private ToolBarController m_ToolBar;
    private TileBase CurrTile;
    private Vector3 CropPosition;
    private Vector3 MousePosition;
    private TreeType m_TreeType;
    public bool CanCut = false;
    
    public TileController GetTileController()
    {
        return m_TileController;
    }

    public void SetTreeTile(TreeType Type)
    {
        m_TreeType = Type;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Player.GetCurrItem() != null && m_Player.CanAction)m_Player.SetAction(m_Player.GetCurrItem().m_Action);
        else m_Player.CanAction = true;

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool MapObjectFind(OnMapObjectController Object)
    {
        foreach(var obj in ObjectOnQueue)
        {
            if(obj.transform.parent.gameObject.name == Object.transform.parent.gameObject.name)
            {
                return false;
            }
        }
        return true;
    }
    private void OnTriggerStay2D(Collider2D other) {
        OnMapObjectController NewObject = other.gameObject.GetComponent<OnMapObjectController>();
        
        if(NewObject != null)
        {
            if(MapObjectFind(NewObject))ObjectOnQueue.Add(NewObject);
            if(NewObject.GetAction() == Action.Cut)
            {
                CanCut = true;
                m_Player.Chop(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        OnMapObjectController NewObject = other.gameObject.GetComponent<OnMapObjectController>();
        
        if(NewObject != null)
        {
            ObjectOnQueue.Remove(NewObject);
            if(ObjectOnQueue.Count == 0)
            {
                CanCut = false;
                m_Player.Chop(false);
            } 
           
        }
    }

    public void TriggerAction(Action m_Action)
    {
        if(m_Action == Action.None)return;
        switch(m_Action)
        {
            case Action.Cut : if(ObjectOnQueue.Count > 0) ObjectOnQueue[0].GetComponent<OnMapObjectController>().SelfDestroy(); break;
            case Action.Hoe : m_TileController.SetCropTile(m_Player, CropPosition); break;
            case Action.Water : m_TileController.SetWateredTile(CropPosition); break;
            case Action.Plant : m_TileController.SetPlantTile(m_Player, CropPosition, m_Player.GetCurrItem().m_TreeType);break;
            default : print("Quen Setup Kia!!!"); break;            
        }
        m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_Durability --;
        if( m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].m_Durability <= 0)
        {
             m_Player.GetInventoryController().Slots[m_ToolBar.GetActiveSlot()].RemoveItem();
        }
        m_ToolBar.Setup();
        // Debug.Break();
    }

    public void SetCropPosition(Vector3 Position)
    {
        CropPosition = Position;
    }

    public bool CanCrop()
    {
        return m_TileController.CanCrop(m_Player, MousePosition);
    }

    public bool CanWater()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanWater(m_Player, MousePosition);
    }

    public bool CanPlant()
    {
        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return m_TileController.CanPlant(m_Player, MousePosition);
    }

}

public enum Action
{
    None,
    Cut,
    Hoe,
    Dig,
    Pick,
    Water,
    Plant,
}
