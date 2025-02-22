using System;
using UnityEngine;

namespace UI
{
    public class InventoryItemPickUp : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        private Action _onPickedUp;

        private void OnEnable()
        {
            _onPickedUp += PickUp;
        }

        private void OnDestroy()
        {
            _onPickedUp -= PickUp;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("PickUp"))
                {
                    _onPickedUp?.Invoke();
                }
            }
        }

        private void PickUp()
        {
            Inventory.instance.AddItem(itemData);
            Debug.Log("Picked up item");
            Destroy(gameObject);
        }
    }
}