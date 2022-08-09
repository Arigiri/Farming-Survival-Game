using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DeadScreen : MonoBehaviour
{
    [SerializeField] private Color ColorSelect;
    [SerializeField] private ButtonController[] Buttons;
    private int NumberSelect;
    private bool Changed = true; // Check if need to update
    void Start()
    {
        NumberSelect = 0;
        ColorSelect.a = 1;
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            NumberSelect -= (int)Input.GetAxisRaw("Vertical");
            Changed = true;
        }
        if(Changed)
        {
            NumberSelect = Math.Max(NumberSelect, 0);
            NumberSelect = Math.Min(NumberSelect, Buttons.Length - 1);
            for(int i = 0; i < Buttons.Length; i++)
            {
                Color color = Color.white;
                if(i == NumberSelect)color = ColorSelect;
                Buttons[i].SetTextColor(color);
            }
            Changed = false;
        }
    }
}
