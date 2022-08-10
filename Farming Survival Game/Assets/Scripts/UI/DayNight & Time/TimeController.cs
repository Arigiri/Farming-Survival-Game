using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeController : MonoBehaviour
{
    [SerializeField]private Volume ppv;
    [SerializeField]private float TotalSecondInDay;
    [SerializeField] private float StartHourOfFirstDay;
    [SerializeField] private RainManager m_RainManager;
    private float TotalSecondInNight;
    private float TotalSecondADay;
    private int Day;
    private float CurrTime;
    private TimeInDay m_TimeInDay;
    private int hour, minute;
    private float tmpHour;
    private void Start() {
        TotalSecondInNight = TotalSecondInDay / 2;
        TotalSecondADay = TotalSecondInDay + TotalSecondInNight;
        CurrTime = ToSystemTime(StartHourOfFirstDay);
        Day = 1;
    }
    // ban ngay tu 4 gio den 20 gio
    // ban den tu 20 gio den 4 gio
    private void Update() {
        CurrTime += Time.deltaTime;
        if(CurrTime > TotalSecondADay)
        {
            CurrTime -= TotalSecondADay;
            Day++;
        }
        if(GetTime() >= 4.0f && GetTime() <= 7.0f)
        {
            // print("Dawn");
            SetUpLight(7.0f - GetTime(), 3.0f);
            m_TimeInDay = TimeInDay.Morning;
        }
        else if(GetTime() >= 18.0f && GetTime() <= 21.0f)
        {
            // print("Night");
            SetUpLight(GetTime() - 18.0f, 3.0f);
            m_TimeInDay = TimeInDay.Night;
        }
        else if(GetTime() < 18.0f && GetTime() > 7.0f)
        {
            // print("Noon");
            SetUpLight(0.0f, 1.0f);
        }
        else 
        {
            // print("MidNight");
            SetUpLight(1.0f, 1.0f);
        }
        // print(CurrTime);

        if(m_RainManager.GetCurrRainLevel() == 2)
        {
            SetLight(0.2f);
        }
        if(m_RainManager.GetCurrRainLevel() == 3)
        {
            SetLight(0.65f);
        }
    }

    private void SetUpLight(float Time, float TotalTime)
    {
        float Weight = Mathf.Min(Time/TotalTime, 1f);
        ppv.weight = Weight;
    }

    public void SetLight(float x)
    {
        float Weight = Mathf.Max(x, ppv.weight);
        ppv.weight = Weight;
    }

    public int GetHour()
    {
        tmpHour = (CurrTime * 24.0f / TotalSecondADay);
        hour = (int)tmpHour;
        return hour;
    }

    public float GetTime()
    {
        return tmpHour;
    }

    public int GetMinute()
    {
        minute = (int)((tmpHour % 1) * 60f);
        minute = (int)(minute/10);
        minute *= 10;
        return minute;
    }

    public int GetDay()
    {
        return Day;
    }

    public float ToSystemTime(float time)
    {
        return time * TotalSecondADay / 24.0f;
    }
}

enum TimeInDay //Các buổi trong ngày
{
    Morning, Night
}