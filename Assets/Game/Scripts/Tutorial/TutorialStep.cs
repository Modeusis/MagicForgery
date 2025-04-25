using System;
using Game.Scripts.TargetMarks;
using UnityEngine;

namespace Game.Scripts.Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        [field: SerializeField] public MarkType TutorialMark { get; private set; }
        
        [field: SerializeField] public bool IsCompleted { get; private set; }
    }
}