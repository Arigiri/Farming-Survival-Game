using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectController : MonoBehaviour
{
    private CollectableType m_Type;
    private int m_MaxStack;
    private Action m_Action;
    private int MaxDurability = -1;
    private int CurrDurability = -1;
    public bool IsTool;
    public CollectableObjectInformation m_Information;
    public CollectableObjectPool m_Pool;
    public Sprite Icon;
    public bool CollectableOrNot;
    public Rigidbody2D rb2d;
    private TreeType m_TreeType;

    private void Awake()
    {
        ResetAttribute(true);
        CollectableOrNot = true;
        m_Pool = FindObjectOfType<CollectableObjectsPool>().m_Pool[m_Type];
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void ResetAttribute(bool ResetDurability)
    {
        m_Type = m_Information.Type;
        m_Action = m_Information.Action;
        m_MaxStack = m_Information.Stack;
        IsTool = m_Information.IsTool;
        Icon = m_Information.Icon;
        if(m_Information.Type == CollectableType.SeedBag)
        {
            m_TreeType = m_Information.m_TreeType;
        }
        if(ResetDurability)
        {
            MaxDurability = m_Information.ToolDurability;
            CurrDurability = m_Information.StartDurability;
        }
        CollectableOrNot = true;
    }
    public CollectableType GetCollectableType()
    {
        return m_Information.Type;
    }

    public TreeType GetTreeType()
    {
        return m_TreeType;
    }
    
    void Start()
    {
       
    }

    public int GetCurrDurability()
    {
        return CurrDurability;
    }

    public void SetDurability(int Durability)
    {
        CurrDurability = Durability;
    }

    private void ChangeCollectableOrNot()
    {
        CollectableOrNot = true;
    }

    public void CallInvokeChangeCollectableOrNot()
    {
        CollectableOrNot = false;
        Invoke("ChangeCollectableOrNot", 1.25f);
    }

    void Update()
    {
        if(IsTool && CurrDurability <= 0)
            SelfDestroy();
    }

    public Collider2D GetCollideWith;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null && other.tag == "Player")
        {
            GetCollideWith = other;
        }

    }

    public void SelfDestroy()
    {
        if(IsTool)
        {
            // gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else m_Pool.Release(this);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null && other.tag == "Player")
            GetCollideWith = null;
    }

    public CollectableObjectPool GetPool()
    {   
        return FindObjectOfType<CollectableObjectsPool>().m_Pool[m_Type];
    }

    public int GetStack()
    {
        return m_MaxStack;
    }

    public Action GetAction()
    {
        return m_Action;
    }


}

public enum CollectableType
{
    NONE, 
    //Farming Productions
    Carrot,
    Chili,
    //Tools
    Axe,
    Pickaxe,
    Shovel,
    Hoe,
    WateringCan,
    SeedBag
}