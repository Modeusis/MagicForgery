using UnityEngine;
using System;

namespace Environment
{
    public class GarbageCan : MonoBehaviour
    {
        private Action _onMoveToTrash;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name == gameObject.name)
                {
                    MoveToTrash();
                }
            }
        }
        
        void MoveToTrash()
        {
            UI.Inventory.instance.RemoveItem();
        }
    }
}