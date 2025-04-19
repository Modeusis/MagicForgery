using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.TargetMarks
{
    public class TargetMarksConfig : ScriptableObject
    {
        [field: SerializeField] public List<TargetMarks> TargetMarks { get; private set; }
    }

    [Serializable]
    public class TargetMarks
    {
        [field: SerializeField] public float TimeToShowMessage { get; private set; }
        [field: SerializeField] public MarkType Target {get; private set;}
        
        [field: SerializeField] public Transform TargetTransform {get; private set;}
        
        [field: SerializeField] public List<string> MessagesForStep {get; private set;}
    }
}