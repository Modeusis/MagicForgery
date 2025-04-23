using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.TargetMarks
{
    [CreateAssetMenu(menuName = "Setups/Target Marks Config")]
    public class TargetMarksConfig : ScriptableObject
    {
        [field: SerializeField] public List<TargetMark> TargetMarks { get; private set; }
    }

    [Serializable]
    public class TargetMark
    {
        [field: SerializeField] public float MessageSymbolsDelay { get; private set; } = 0.2f;
        [field: SerializeField] public MarkType Target {get; private set;}
        
        [field: SerializeField] public Vector3 TargetPosition {get; private set;}
        
        [field: SerializeField] public string MessagesForStep {get; private set;}
    }
}