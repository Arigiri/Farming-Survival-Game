using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChestController : MonoBehaviour
{
    public CollectableObjectController[] ItemsContainer;
    [SerializeField] private Action m_Action;
    [SerializeField] private float m_MaxLengthInteractive;
    private TileController m_TileController;
    private PlayerController m_Player;
    private ChestUI m_ChestUI;
    public void Init()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_ChestUI = GameObject.FindGameObjectWithTag("ChestUI").GetComponent<ChestUI>();
    }

    public void OpenChest() // Mo ruong
    {
        m_ChestUI.TurnOnChestUI();
    }

    public void SelfDestroy() // Pha ruong va Lay ruong vao Inventory
    {
        m_Player.AddItemToInventory(gameObject.GetComponent<CollectableObjectController>());
        DropItem();
        transform.parent.gameObject.SetActive(false);
        m_TileController.RemoveOnMapObject(transform.position);
    }
    private void DropItem()
    {
        Vector3 SpawnPoint = m_Player.RandomPointInAnnulus(transform.position, 0.35f, 0.5f);
        foreach(CollectableObjectController item in ItemsContainer)
        {
            item.ResetAttribute(true);
            Vector3 SpawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            m_Player.DropAllFromObject(item, SpawnPoint + SpawnOffset, SpawnPoint);
        }
    }
}
