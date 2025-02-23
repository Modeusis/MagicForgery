using UnityEngine;
using System;
using UI;

namespace Environment
{
    public class GarbageCan : MonoBehaviour
    {
        [SerializeField] private AudioClip toTrashSound;

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
            Inventory.instance.RemoveItem();
            SoundManager.instance.PlaySfx(toTrashSound);
        }
    }
}