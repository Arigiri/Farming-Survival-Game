using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Scroller : MonoBehaviour
{
    [SerializeField] private Transform[] m_SpawnPoint;
    [SerializeField] private CloudMenuBackground[] m_Clouds;
    private float RealTime, StartTime;
    void Start()
    {
        foreach(CloudMenuBackground i in m_Clouds)
        {
            i.CallInit();
        }
    }
    void Update()
    {

    }
}
