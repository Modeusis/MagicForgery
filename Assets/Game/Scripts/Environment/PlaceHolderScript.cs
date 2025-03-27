using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    [RequireComponent(typeof(Animator))]
    public class PlaceHolderScript : MonoBehaviour
    {
        [Header("Sword placing")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private GameObject swordPlace;

        [Header("Enchanted sword replacer")]
        [SerializeField] private GameObject swordPackagePrefab;
        [SerializeField] private Sprite swordPackageSprite;
        [SerializeField] private Material packageMaterial;
        
        private ItemGenerator _itemGenerator = new ItemGenerator();
        private Sword _tempEnchantedSword;
        private string _swordPackageName = "Enchanted sword";
        
        public bool IsBlocked { get; set; }
        
        private Sword _placedSword;

        public Sword PlacedSword
        {
            get => _placedSword;
            set
            {
                if (_placedSword == value)
                    return;
                _placedSword = value;
                MagicEnchanterController.Instance.SwordToEnchant = _placedSword;
            }
        }
        
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
                if (SwordPlaceToggle())
                {
                    _isSwordPlaced = value;
                    TooltipController.Instance.TooltipMessage = $"{interactKey.ToString()} to {(_isSwordPlaced ? "take" : "place")} sword";
                }
                else
                {
                    TooltipController.Instance.ShowMechanicsDescription("Select sword to place");
                    _item = null;
                }
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
                if (_placeHolderAnimator)
                {
                    _placeHolderAnimator.SetBool("IsOpened", IsPlaceHolderOpened); 
                }
                
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
        
        private void Awake()
        {
            _placeHolderAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled && !IsBlocked && !Player.Player.instance.IsOverlayShowed)
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
                                    if (Player.Player.instance.selectedItem.prefab.GetComponent<Sword>())
                                    {
                                        IsSwordPlaced = true;
                                    }
                                    else
                                    {
                                        TooltipController.Instance.ShowMechanicsDescription("Select sword to place");
                                    }
                                }
                            }
                            else
                            {
                                TooltipController.Instance.ShowMechanicsDescription("Open it with lever first");
                            }
                        }
                        else if (IsSwordPlaced && _item)
                        {
                            if (IsPlaceHolderOpened)
                            {
                                IsSwordPlaced = false;
                            }
                            else
                            {
                                TooltipController.Instance.ShowMechanicsDescription("Open it with lever first");
                            }
                        }
                    }
                }
                else
                {
                    IsPlaceHolderInFocus = false;
                }
            }
        }
        
        private bool SwordPlaceToggle()
        {
            if (!IsSwordPlaced)
            {
                _item = Player.Player.instance.selectedItem;
                if (!_item.prefab.GetComponent<Sword>())
                {
                    return false;
                }
                Inventory.instance.RemoveItem();
                _itemPrefab = Instantiate(_item.prefab, swordPlace.transform);
                Destroy(_itemPrefab.GetComponent<InventoryItemPickUp>());
                Destroy(_itemPrefab.GetComponent<BoxCollider>());
                PlacedSword = _itemPrefab.GetComponent<Sword>();
                if (_item.itemName == _swordPackageName)
                {
                    _itemPrefab.transform.localPosition = new Vector3(-0.6f, 0.1f, 0);
                    _itemPrefab.transform.localRotation = Quaternion.Euler(-90, 0f, -90);
                    _itemPrefab.transform.localScale = new Vector3(0.7f, 0.77f, 1f);
                }
                else
                {
                    _itemPrefab.transform.localPosition = new Vector3(-0.8f, 0f, 0);
                    _itemPrefab.transform.localRotation = new Quaternion(0.5f,0.5f,-0.5f,0.5f);
                    _itemPrefab.transform.localScale = new Vector3(1.5f, 2f, 1.5f);
                }
                _itemPrefab.SetActive(true);
            }
            else
            {
                Inventory.instance.AddItem(_item);
                Destroy(_itemPrefab);
                _itemPrefab = null;
                _item = null;
                PlacedSword = null;
            }
            
            return true;
        }

        public void OnSwordEnchanted(Sword enchantedSword)
        {
            Destroy(_itemPrefab);
            _itemPrefab = null;
            _item = null;
            
            var tempPrefab = swordPackagePrefab;
            var component = tempPrefab.GetComponent<Sword>();
            
            component.CopyFrom(enchantedSword);
            
            _itemGenerator.SetItemData(0, _swordPackageName, swordPackageSprite, "Enchanted sword", tempPrefab, 0.4f, Quaternion.Euler(0f, 270f, 0f));
            var itemInfo = _itemGenerator.GenerateItem();

            _item = itemInfo;
            _itemPrefab = Instantiate(_item.prefab, swordPlace.transform);
            PlacedSword = _itemPrefab.GetComponent<Sword>();
            _itemPrefab.transform.localPosition = new Vector3(-0.6f, 0.1f, 0);
            _itemPrefab.transform.localRotation = Quaternion.Euler(-90, 0f, -90);
            _itemPrefab.transform.localScale = new Vector3(0.7f, 0.77f, 1f);
            _itemPrefab.SetActive(true);
        }
    }
}