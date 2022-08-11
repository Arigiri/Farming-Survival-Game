using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Rain;
    [SerializeField] private ParticleSystem m_RainSplash;
    
    private int CurrRainLevel;

    public int GetCurrRainLevel()
    {
        return CurrRainLevel;
    }

    public void SetRainLevel(int level)
    {
        level = Mathf.Max(level, 0);
        level = Mathf.Min(level, 3);

        if(level == 0)
        {
            var RainEmission = m_Rain.emission;
            RainEmission.rateOverTime = 0;
            var RainSplashEmission = m_RainSplash.emission;
            RainSplashEmission.rateOverTime = 0;
        }
        if(level == 1)
        {
            var RainEmission = m_Rain.emission;
            RainEmission.rateOverTime = 125;
            var RainSplashEmission = m_RainSplash.emission;
            RainSplashEmission.rateOverTime = 30;
            var RainGravityModifier = m_Rain.main;
            RainGravityModifier.gravityModifier = 1.4f;
        }
        if(level == 2)
        {
            var RainEmission = m_Rain.emission;
            RainEmission.rateOverTime = 250;
            var RainSplashEmission = m_RainSplash.emission;
            RainSplashEmission.rateOverTime = 60;
            var RainGravityModifier = m_Rain.main;
            RainGravityModifier.gravityModifier = 1.75f;
        }
        if(level == 3)
        {
            var RainEmission = m_Rain.emission;
            RainEmission.rateOverTime = 500;
            var RainSplashEmission = m_RainSplash.emission;
            RainSplashEmission.rateOverTime = 120;
            var RainGravityModifier = m_Rain.main;
            RainGravityModifier.gravityModifier = 2.25f;
        }
        CurrRainLevel = level;
    }
}
