using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSystem : MonoBehaviour
{
    [SerializeField] private Key[] keys;
    [SerializeField] private Keybindings keybindings;
    [SerializeField] private DefaultKeybindings m_DefaultKeybindings;
    // [SerializeField] private GameObject m_object;

    private void Start()
    {
        // transform.position = m_object.transform.position;
        for(int i = 0; i < keys.Length; i++)
        {
            keys[i].KeybindingIndex = i;
        }
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
    public void SetKeyCode(int idx, KeyCode key)
    {
        // 2 cai duoi day thay doi UI
        keys[idx].SetKeyCode(key);
        keys[idx].Setup();

        // Duoi day thay doi KeyCode trong game
        keybindings.ChangeKeybinding(keys[idx].GetKeybindingActions(), keys[idx].GetKeyCode());
    }
}
