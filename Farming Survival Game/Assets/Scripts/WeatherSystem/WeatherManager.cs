using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private int m_RainLevel; // = 0 neu khong mua, 1, 2, 3 tuong ung voi cac cap do mua tang dan
    [SerializeField] private bool m_DoThunder;// Cai sam chop nay tu dong neu troi mua cap 3, cap <= 2 kh co sam chop
    [SerializeField] private RainManager m_RainManager;
    [SerializeField] private ThunderManager m_ThunderManager;

    private int ThunderGapTime;
    // Start is called before the first frame update
    void Start()
    {
        m_RainManager.SetRainLevel(0);
        m_DoThunder = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_RainLevel != m_RainManager.GetCurrRainLevel())
        {
            m_RainManager.SetRainLevel(m_RainLevel);
        }
        if(m_RainLevel >= 3 && m_DoThunder)
        {
            m_DoThunder = false;
            m_ThunderManager.DoThunder();
            ThunderGapTime = Random.Range(10, 60);
            Invoke("SetDoThunderTrue", ThunderGapTime);
        }
    }

    private void SetDoThunderTrue()
    {
        m_DoThunder = true;
    }
}
