using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.AimMarks
{
    public class AimMarksConfig : ScriptableObject
    {
        [field: SerializeField] public MarkType Aim {get; private set;}
        
        [field: SerializeField] public Transform AimTransform {get; private set;}
        
        [field: SerializeField] public List<string> MessagesForStep {get; private set;}
    }
}