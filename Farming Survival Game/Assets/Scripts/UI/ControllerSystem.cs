using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSystem : MonoBehaviour
{
    [SerializeField] private Key[] keys;
    [SerializeField] private Keybindings keybindings;

    private void Start()
    {
        int idx = 0;
        foreach(Keybindings.KeybindingsCheck key in keybindings.keybindingsCheck)
        {
            if(idx >= keys.Length)
            {
                print("ALERT!!! THIEU SLOT CHO KEYBINDING TRONG PANEL KEYBINDINGS KIA"); // cai nay khong can xoa dau, de con debug
                return;
            }
            keys[idx].SetKeybindingActions(key.keybindingActions);
            keys[idx].SetKeyCode(key.keyCode);
            idx++;
        }
        Setup();
    }

    public void Setup()
    {
        foreach(Key key in keys)
        {
            key.Setup();
        }
    }
}
