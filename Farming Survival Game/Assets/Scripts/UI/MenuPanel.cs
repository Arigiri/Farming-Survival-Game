using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Color MenuColorSelect;
    [SerializeField] private ButtonController[] Buttons;
    [SerializeField] private GameObject m_MenuBackgroundButtons;

    private int MenuNumberSelect;
    private bool Changed = true; // Check if need to update
    // private bool Trigger = false;
    void Start()
    {
        MenuNumberSelect = 0;
        MenuColorSelect.a = 1;
    }

    void Update()
    {
        if(gameObject.activeSelf == false)
            return;
        if(m_MenuBackgroundButtons.gameObject.activeSelf == false)
            return;
        if(Input.anyKeyDown)
        {
            if(InputManager.instance.GetKeyDown(KeybindingActions.Select))
            {
                Buttons[MenuNumberSelect].Trigger();
            }

            MenuNumberSelect -= (int)Input.GetAxisRaw("Vertical");
            int Temp = (int)Input.GetAxisRaw("Horizontal") * Buttons.Length/2;
            
            if(MenuNumberSelect < Buttons.Length/2)
                if(Temp > 0)MenuNumberSelect += Temp;
                else {}
            else if(MenuNumberSelect >= Buttons.Length/2) 
                if(Temp < 0)MenuNumberSelect += Temp;
            Changed = true;
        }
        if(Changed)
        {
            MenuNumberSelect = Math.Max(MenuNumberSelect, 0);
            MenuNumberSelect = Math.Min(MenuNumberSelect, Buttons.Length - 1);
            for(int i = 0; i < Buttons.Length; i++)
            {
                Color color = Buttons[i].DefaultColor;
                if(i == MenuNumberSelect)color = MenuColorSelect;
                Buttons[i].SetTextColor(color);
            }
            Changed = false;
        }
    }
}