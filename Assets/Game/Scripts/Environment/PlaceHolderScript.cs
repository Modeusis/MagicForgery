using UI;
using UnityEngine;

namespace Environment
{
    public class PlaceHolderScript : MonoBehaviour
    {
        //Пофиксить доставание через коробку меча
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        [SerializeField] private GameObject swordPlace;

        public bool isPlaceHolderBlocked;
        
        private Animator _placeHolderAnimator;
        private GameObject _itemPrefab;
        private ItemData _item;
        
        private bool _isSwordPlaced;
        private bool _isPlaceHolderOpened;
        private bool _isPlaceHolderInFocus;
        
        public bool IsSwordPlaced
        {
            get => _isSwordPlaced;
            set
            {
                if (_isSwordPlaced == value)
                    return;
                _isSwordPlaced = value;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isSwordPlaced ? "take" : "place")} sword";
                SwordPlaceToggle();
            }
        }
        
        public bool IsPlaceHolderOpened
        {
            get => _isPlaceHolderOpened;
            set
            {
                if (_isPlaceHolderOpened == value)
                    return;
                _isPlaceHolderOpened = value;
                _placeHolderAnimator.SetBool("IsOpened", IsPlaceHolderOpened); 
            }
        }
        
        private bool IsPlaceHolderInFocus
        {
            get => _isPlaceHolderInFocus;
            set
            {
                if (_isPlaceHolderInFocus == value)
                    return;
                _isPlaceHolderInFocus = value;
                TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isSwordPlaced ? "take" : "place")} sword";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                swordPlace.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }
        
        void Awake()
        {
            _placeHolderAnimator = GetComponent<Animator>();
        }

        void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled && !isPlaceHolderBlocked)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.gameObject.CompareTag("PlaceHolder"))
                {
                    IsPlaceHolderInFocus = true;
                    if (Input.GetKeyDown(interactKey))
                    {
                        if (!IsSwordPlaced)
                        {
                            if (IsPlaceHolderOpened)
                            {
                                if  (Player.Player.instance.selectedItem)
                                {
                                    IsSwordPlaced = true;
                                }
                                else
                                {
                                    TooltipController.Instance.ShowMechanicsDescription("Select sword to place");
                                }
                            }
                            else
                            {
                                TooltipController.Instance.ShowMechanicsDescription("Open it with lever first");
                            }
                        }
                        else if (IsSwordPlaced && _item)
                        {
                            IsSwordPlaced = false;
                        }
                    }
                }
                else
                {
                    IsPlaceHolderInFocus = false;
                }
            }
        }
        
        void SwordPlaceToggle()
        {
            if (IsSwordPlaced)
            {
                if (Player.Player.instance.selectedItem.itemName != "Sword")
                    return;
                _item = Player.Player.instance.selectedItem;
                Inventory.instance.RemoveItem();
                _itemPrefab = Instantiate(_item.prefab, swordPlace.transform);
                Destroy(_itemPrefab.GetComponent<InventoryItemPickUp>());
                Destroy(_itemPrefab.GetComponent<BoxCollider>());
                _itemPrefab.transform.localPosition = new Vector3(-0.8f, 0f, 0);
                _itemPrefab.transform.localRotation = new Quaternion(0.5f,0.5f,-0.5f,0.5f);
                _itemPrefab.transform.localScale = new Vector3(1.5f, 2f, 1.5f);
                _itemPrefab.SetActive(true);
                IsSwordPlaced = true;
            }
            else
            {
                Debug.Log("Taken sword");
                Inventory.instance.AddItem(_item);
                Destroy(_itemPrefab);
                _itemPrefab = null;
                _item = null;
            }
        }
    }
}