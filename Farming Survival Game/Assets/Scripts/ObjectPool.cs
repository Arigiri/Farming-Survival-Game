using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectPool
{
    public List<CollectableObjectController> m_ActiveObject = new List<CollectableObjectController>();
    public List<CollectableObjectController> m_InactiveObject = new List<CollectableObjectController>();
    public CollectableType m_PoolType;
    public CollectableObjectController m_Object;

    public CollectableObjectPool(CollectableType Type)
    {
        m_PoolType = Type;
    }
    public CollectableObjectController Spawn(Vector2 Position, Transform Parent, CollectableObjectController item)
    {
        CollectableObjectController m_NewObj;
        if(item.IsTool)
        {
            m_NewObj = GameObject.Instantiate(m_Object);
            m_NewObj.transform.position = Position;
            if(item.GetCurrDurability() < 0)m_NewObj.ResetAttribute(true);
            else m_NewObj.SetDurability(item.GetCurrDurability());
            m_NewObj.transform.SetParent(Parent);
        }
        else 
        if(m_InactiveObject.Count <= 0)
        {
            m_NewObj = GameObject.Instantiate(item);
            m_NewObj.transform.position = Position;
            m_NewObj.transform.SetParent(Parent);
            m_NewObj.gameObject.SetActive(true);
            m_ActiveObject.Add(m_NewObj);
        }
        else
        {
            m_NewObj = m_InactiveObject[0];
            m_NewObj.transform.position = Position;
            m_NewObj.transform.SetParent(Parent);
            m_NewObj.gameObject.SetActive(true);
            m_ActiveObject.Add(m_NewObj);
            m_InactiveObject.RemoveAt(0);
            
        }
        return m_NewObj;
    }

    public void Release(CollectableObjectController m_Obj)
    {
        m_Obj.gameObject.SetActive(false);
        m_InactiveObject.Add(m_Obj);
        m_ActiveObject.Remove(m_Obj);
    }
}