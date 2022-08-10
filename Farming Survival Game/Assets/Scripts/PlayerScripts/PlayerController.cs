using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator m_Animator;
    [SerializeField] private InputAction m_InputAction;
    [SerializeField] private float m_movespeed;
    [SerializeField] private InventoryController m_Inventory;
    [SerializeField] private float m_Scale;
    [SerializeField] private GameObject m_ChopAnimation;
    [SerializeField] private AttributeController m_AttributeController;
    [SerializeField] private CollectableObjectsPool m_CollectableObjectsPool;
    [SerializeField] private AttributeUIController m_AttributeUIController;
    [SerializeField] private PlayerActionController m_PlayerActionController;

    private Vector2 m_MoveDirection = Vector2.zero;
    private Rigidbody2D rb;
    private PlayerActionController m_ActionCollider;
    private InventoryController.Slot CurrItemOnHand;
    private Action m_Action;
    private InventoryController.Slot TempItemOnHand;
    public bool CanAction = true;
    public bool Active = true;
    public float CurrHealth, CurrFood, CurrStamina;
    public float MaxHealth, MaxFood, MaxStamina;

    private void OnEnable() 
    {
        m_InputAction.Enable();
    }

    private void OnDisable() 
    {
        m_InputAction.Disable();
    }

    public InventoryController GetInventoryController()
    {
        return m_Inventory;
    }
    
    void Start()
    {
        CurrHealth = MaxHealth;
        CurrFood = MaxFood;
        CurrStamina = MaxStamina;

        m_AttributeController.MaxHealth = MaxHealth;
        m_AttributeController.MaxFood = MaxFood;
        m_AttributeController.MaxStamina = MaxStamina;

        m_ActionCollider = gameObject.GetComponentInChildren<PlayerActionController>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    public InventoryController.Slot GetCurrItem()
    {
        return CurrItemOnHand;
    }
    public void SetCurrItem(InventoryController.Slot item)
    {
        CurrItemOnHand = item;
    }
    void Update()
    {
        if(!Active)return;
        //Check Current item on hand 
        if(CurrItemOnHand != null && CurrItemOnHand.Count == 0)CurrItemOnHand = null;
        //trigger action
        if(Input.anyKeyDown)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                CanAction = true;
                try
                {
                    if(CurrItemOnHand.m_Action == m_Action)
                    {
                        var Position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        if(m_Action == Action.Hoe && m_PlayerActionController.CanCrop())
                        {
                            m_AttributeUIController.MakeProgressBar(0.2f);
                            m_AttributeUIController.SetAction(m_Action);
                            m_PlayerActionController.SetCropPosition(Position);
                        }
                        else if(m_Action == Action.Cut && m_PlayerActionController.CanCut)
                        {
                            m_AttributeUIController.MakeProgressBar(0.2f);
                            m_AttributeUIController.SetAction(m_Action);
                        }
                        else if(m_Action == Action.Water && m_PlayerActionController.CanWater())
                        {
                            m_AttributeUIController.MakeProgressBar(0.2f);
                            m_AttributeUIController.SetAction(m_Action);
                            m_PlayerActionController.SetCropPosition(Position);
                        }
                        else if(m_Action == Action.Plant && m_PlayerActionController.CanPlant())
                        {
                            m_AttributeUIController.MakeProgressBar(0.2f);
                            m_AttributeUIController.SetAction(m_Action);
                            m_PlayerActionController.SetCropPosition(Position);
                        }
                        TempItemOnHand = CurrItemOnHand;
                    }
                    
                    
                }
                catch{}
            }
        }
        
        if(m_Action == Action.None || TempItemOnHand != CurrItemOnHand)
        {
            m_AttributeUIController.TurnOffProgressBar();
            TempItemOnHand = null;
        }
        
        //Update Attribute Information
        m_AttributeController.CurrHealth = CurrHealth;
        m_AttributeController.CurrFood = CurrFood;
        m_AttributeController.CurrStamina = CurrStamina;

        //Movement
        m_MoveDirection = m_InputAction.ReadValue<Vector2>();
        if(m_MoveDirection != Vector2.zero)
        {
            Run();
            SetAction(Action.None); //reset action
            CanAction = false;
        }
        else
            Idle();
        if(m_MoveDirection.x > 0)
            transform.localScale = Vector3.one * m_Scale;
        else if (m_MoveDirection.x < 0)
            transform.localScale= new Vector3(-1, 1, 1) * m_Scale;
    }

    public float GetMoveDirection()
    {
        return transform.localScale.x;
    }

    private void FixedUpdate() 
    {
        rb.velocity = m_MoveDirection * m_movespeed;
    }

    [ContextMenu("Run")]
    private void Run()
    {
        m_Animator.SetBool("Run", true);
        m_Animator.SetBool("Idle", false);
    }
    [ContextMenu("Idle")]
    private void Idle()
    {
        m_Animator.SetBool("Idle", true);
        m_Animator.SetBool("Run", false);
    }
    [ContextMenu("Die")]
    private void Die()
    {
        m_Animator.SetBool("Die", true);
        m_Animator.SetBool("Idle", false);
        m_Animator.SetBool("Run", false);
    }

    private void OnTriggerStay2D(Collider2D other) {
        CollectableObjectController m_object = other.GetComponent<CollectableObjectController>();
        
        if(m_object != null && m_object.tag == "CollectableObject" && m_object.GetCollideWith == gameObject.GetComponent<Collider2D>() && m_object.CollectableOrNot)
        {
            if(m_Inventory.Add(m_object))   m_object.SelfDestroy();
        }
    }

    public int GetInventoryNumSlot()
    {
        return m_Inventory.Slots.Count;
    }

    public CollectableType GetCollectableType(int idx)
    {
        return m_Inventory.Slots[idx].Type;
    }

    public int GetCollectableCount(int idx)
    {
        return m_Inventory.Slots[idx].Count;
    }

    public InventoryController.Slot GetSlot(int idx)
    {
        return m_Inventory.Slots[idx];
    }

    public void Remove(int SlotId)
    {
        m_Inventory.Remove(SlotId);
    }

    public void Chop(bool StartChop)
    {
        m_ChopAnimation.SetActive(StartChop);
    }
    public void DropItem(CollectableObjectController item)
    {
        Vector3 spawnLocation = transform.position;

        Vector3 spawnPoint = RandomPointInAnnulus(spawnLocation, 0.25f, 0.5f);
       
        CollectableObjectController droppedItem = m_CollectableObjectsPool.m_Pool[item.GetCollectableType()].Spawn(spawnPoint,null, item);//Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce((spawnPoint - spawnLocation) * 3f, ForceMode2D.Impulse);
        ChangeCollectableOrNot(droppedItem);
    }

    public void DropAllItem(CollectableObjectController item, Vector3 spawnPoint)
    {
         print(item.GetCurrDurability());
        Vector3 spawnLocation = transform.position;
        CollectableObjectController droppedItem = m_CollectableObjectsPool.m_Pool[item.GetCollectableType()].Spawn(spawnPoint,null, item);//Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce((spawnPoint - spawnLocation) * 3f, ForceMode2D.Impulse);
        ChangeCollectableOrNot(droppedItem);
    }

    private void ChangeCollectableOrNot(CollectableObjectController item)
    {
        item.CallInvokeChangeCollectableOrNot();
    }

    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
    {
 
        var randomDirection = (UnityEngine.Random.insideUnitCircle * origin).normalized;
 
        var randomDistance = UnityEngine.Random.Range(minRadius, maxRadius);
 
        var point = origin + randomDirection * randomDistance;
 
        return point;
    }

    public void DropItemAhead(CollectableObjectController item)
    {
        Vector3 spawnLocation = transform.position;

        Vector3 spawnPoint = RandomPointAheadPlayer(spawnLocation, 0.25f, 0.5f);

        CollectableObjectController droppedItem = m_CollectableObjectsPool.m_Pool[item.GetCollectableType()].Spawn(spawnPoint,null, item);//Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce((spawnPoint - spawnLocation) * 3f, ForceMode2D.Impulse);
        ChangeCollectableOrNot(droppedItem);
    }

    public Vector2 RandomPointAheadPlayer(Vector2 origin, float minRadius, float maxRadius)
    {
 
        var randomDirection = new Vector2 (GetMoveDirection(), 1).normalized;
 
        var randomDistance = UnityEngine.Random.Range(minRadius, maxRadius);
 
        var point = origin + randomDirection * randomDistance;
 
        return point;
    }

    public void DropAllFromObject(CollectableObjectController item, Vector3 spawnPoint, GameObject obj)
    {
        Vector3 spawnLocation = obj.transform.position;
        CollectableObjectController droppedItem = m_CollectableObjectsPool.m_Pool[item.GetCollectableType()].Spawn(spawnPoint,null, item);//Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
        droppedItem.rb2d.AddForce((spawnPoint - spawnLocation) * 2.5f, ForceMode2D.Impulse);
        ChangeCollectableOrNot(droppedItem);
    }

    public void SetItemOnHand(InventoryController.Slot item)
    {
        CurrItemOnHand = item;
    }

    public void SetAction(Action action)
    {
        m_Action = action;
    }
    public void InventorySwap(int idx1, int idx2)
    {
        m_Inventory.Swap(idx1, idx2);
    }
    public Action GetAction()
    {
        return m_Action;
    }
}
