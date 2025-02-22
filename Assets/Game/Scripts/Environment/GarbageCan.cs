using UnityEngine;
using System;

namespace Environment
{
    public class GarbageCan : MonoBehaviour
    {
        private Action _onMoveToTrash;

        private void OnEnable()
        {
            _onMoveToTrash += MoveToTrash;
        }

        private void OnDisable()
        {
            _onMoveToTrash -= MoveToTrash;
        }

        void MoveToTrash()
        {
            
        }
    }
}