using System;
using UnityEngine;

namespace Game.Scripts.AI
{
    [Serializable]
    public class CustomerDestination
    {
        [field: SerializeField] public Vector3 DestinationPosition { get; private set; }
        
        [field: SerializeField] public DestinationType CurrentDestination { get; private set; }
        [field: SerializeField] public DestinationType NextDestinationType { get; private set; }
    }
}