using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.MiniActivities
{
    [Serializable]
    public class WordBlock
    {
        [field:SerializeField] public TMP_Text TextBlock{ get; set; } 
        [field:SerializeField] public bool IsShowed{ get; set; } 
        [field:SerializeField] public string TextBlockValue{ get; set; } 
    }
}