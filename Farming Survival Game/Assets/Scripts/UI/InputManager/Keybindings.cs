using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    [System.Serializable]
    public class KeybindingsCheck
    {
        public KeybindingActions keybindingActions;
        public KeyCode keyCode;
    }

    public KeybindingsCheck[] keybindingsCheck;

    public void ChangeKeybinding(KeybindingActions keybindingActions, KeyCode keyCode)
    {
        foreach(KeybindingsCheck keybinding in keybindingsCheck)
        {
            if(keybinding.keybindingActions == keybindingActions)
            {
                keybinding.keyCode = keyCode;
            }
        }
    }
}
