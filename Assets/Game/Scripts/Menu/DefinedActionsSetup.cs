using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.MainMenu
{
    [CreateAssetMenu(menuName = "Setups/Setting menu definer")]
    public class DefinedActionsSetup : ScriptableObject
    {
        [field: SerializeField] public List<DefinedAction> Actions { get; private set; }
    }

    [Serializable]
    public class DefinedAction
    {
        [field: SerializeField] public MenuAction Type {get; private set;}
        [field: SerializeField] public string PopUpDescription {get; private set;}
    }
}