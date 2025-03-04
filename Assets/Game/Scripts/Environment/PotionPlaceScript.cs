using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class PotionPlaceScript : MonoBehaviour, IToggle
    {
        private ItemData _potionData;
        private GameObject _potionMesh;
        
        public MagicEnchanterController.PotionType? PlacedPotionType { get; set; }

        [SerializeField] private float potionScale = 1f; 
        [SerializeField] private Vector3 potionPosition = Vector3.zero;
        [SerializeField] private Quaternion potionRotation = Quaternion.identity;
        
        public ItemData PotionData
        {
            get => _potionData;
            set
            {
                if (_potionData == value) 
                    return;
                if (!value.isPotion)
                    return;
                _potionData = value;
                switch (_potionData.itemName)
                {
                    case "Red potion":
                        PlacedPotionType = MagicEnchanterController.PotionType.Red;
                        break;
                    case "Ice potion":
                        PlacedPotionType = MagicEnchanterController.PotionType.Blue;
                        break;
                    case "Nature potion":
                        PlacedPotionType = MagicEnchanterController.PotionType.Green;
                        break;
                    case "Lightning potion":
                        PlacedPotionType = MagicEnchanterController.PotionType.Yellow;
                        break;
                    case "Metal potion":
                        PlacedPotionType = MagicEnchanterController.PotionType.Metal;
                        break;
                    default:
                        PlacedPotionType = null;
                        return;
                }
            }
        }

        private bool _isFocused;

        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                TooltipController.Instance.TooltipMessage =
                    $"Press {Player.Player.instance.InteractKey} to {(_isToggled ? "take" : "place")} potion";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }
        
        private bool _isToggled;

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                var selectedItem = Player.Player.instance.selectedItem;
                if (!_isToggled)
                {
                    if (selectedItem)
                    {
                        if (selectedItem.isPotion)
                        {
                            PotionData = selectedItem;
                            Inventory.instance.RemoveItem();
                            
                            _potionMesh = Instantiate(PotionData.prefab, gameObject.transform);
                            Destroy(_potionMesh.GetComponent<BoxCollider>());
                            _potionMesh.transform.localPosition = potionPosition;
                            _potionMesh.transform.localRotation = potionRotation;
                            _potionMesh.transform.localScale = Vector3.one * potionScale;
                        }
                        else
                        {
                            TooltipController.Instance.ShowMechanicsDescription("Unable to do this");
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log("First return");
                        return;
                    }
                }
                else
                {
                    if (PotionData)
                    {
                        if (Inventory.instance.AddItem(PotionData))
                        {
                            Destroy(_potionMesh);
                            _potionMesh = null;
                            PotionData = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                _isToggled = value;
                TooltipController.Instance.TooltipMessage =
                    $"Press {Player.Player.instance.InteractKey} to {(_isToggled ? "take" : "place")} potion";
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }
    }
}