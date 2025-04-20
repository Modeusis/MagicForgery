using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Game.Scripts.MiniActivities
{
    [CreateAssetMenu(menuName = "Setups/Words Data Config")]
    public class WordsData : ScriptableObject
    {
        [field:SerializeField] public List<string> Words { get; private set; }
    }
}