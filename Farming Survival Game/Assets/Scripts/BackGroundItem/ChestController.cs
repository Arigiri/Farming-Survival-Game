using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestController : MonoBehaviour
{
    public CollectableObjectController[] ItemsContainer;
    [SerializeField]private Action m_Action;
    [SerializeField] private PlayerController m_Player;
    [SerializeField] private float m_MaxLengthInteractive;
    public Tilemap m_TileMap;
    private TileController m_TileController;
    private Vector3Int CurrTile;
    // Start is called before the first frame update
    void Start()
    {
        m_TileController = FindObjectOfType<TileController>();
        CurrTile = m_TileController.GetTile(transform.position, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            if(InputManager.instance.GetKeyDown(KeybindingActions.Interact))
            {
                if(CanOpenChest())
                {
                    
                }
            }
        }
    }

    public bool CanOpenChest()
    {
        Vector3 MousePosiotion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int PlayerLocation = m_TileMap.WorldToCell(m_Player.transform.position);
        Vector3Int NewLocation = m_TileMap.WorldToCell(MousePosiotion);
        return ((PlayerLocation - NewLocation).magnitude <= m_MaxLengthInteractive && m_TileController.CanPutObject(NewLocation));
    }

    public void SelfDestroy()
    {
        m_Player.AddItemToInventory(gameObject.GetComponent<CollectableObjectController>());
        DropItem();
        transform.parent.gameObject.SetActive(false);
        m_TileController.RemoveOnMapObject(transform.position);
    }
    private void DropItem()
    {
        PlayerController m_Player = FindObjectOfType<PlayerController>();
        Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(transform.position, 0.35f, 0.5f);
        foreach(CollectableObjectController item in ItemsContainer)
        {
            item.ResetAttribute(true);
            Vector3 SpawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            m_Player.DropAllFromObject(item, SpawnPoint + SpawnOffset, SpawnPoint);
        }
    }
    public Action GetAction()
    {
        return m_Action;
    }

    public Vector3Int GetTile()
    {
        return CurrTile;
    }
}
