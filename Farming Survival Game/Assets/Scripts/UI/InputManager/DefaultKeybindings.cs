using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DefaultKeybindings", menuName = "DefaultKeybindings")]
public class DefaultKeybindings : ScriptableObject
{
    public Keybindings.KeybindingsCheck[] keybindingsCheck;
}