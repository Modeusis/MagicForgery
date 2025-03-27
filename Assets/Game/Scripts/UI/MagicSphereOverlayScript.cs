using System;
using System.Collections.Generic;
using DG.Tweening;
using Environment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MagicSphereOverlayScript : MonoBehaviour
    {
        [Header("Panel labels")]
        [SerializeField] private GameObject swordSpriteBlock;
        [SerializeField] private TMP_Text swordName;
        [SerializeField] private TMP_Text swordInfo;
        [SerializeField] private TMP_Text converterEnchantmentName;
        [SerializeField] private TMP_Text enchantmentName;
        [SerializeField] private List<TMP_Text> swordStats;
        
        [Header("Hover tooltip")]
        [SerializeField] private TMP_Text uiTooltip;
        [SerializeField] private GameObject tooltipBackground;
        [SerializeField] private Button startEnchantmentButton;
        
        [Header("Key elements")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private MagicSphereScript rootSphereScript;
        
        [Header("Content")]
        [SerializeField] private Sprite undefinedSprite;
        
        [Header("Drawing")]
        [SerializeField] private GameObject drawPlace;
        
        private Image _swordImage;
        private CanvasGroup _canvasGroup;
        private Sword _placedSword;
        private Enchantment _currentEnchantment;

        private Sword PlacedSword
        {
            get => _placedSword;
            set
            {
                if (_placedSword == value)
                    return;
                
                _placedSword = value;
                
                if (_placedSword && _swordImage)
                {
                    _swordImage.sprite = _placedSword.SwordIcon;
                }
            }
        }

        private Enchantment CurrentEnchantment
        {
            get => _currentEnchantment;
            set
            {
                if (_currentEnchantment == value)
                    return;
                
                _currentEnchantment = value;
            }
        }
        
        private bool _isTooltipShown;
        private bool IsTooltipShown
        {
            get => _isTooltipShown;
            set
            {
                if (_isTooltipShown == value)
                    return;
                _isTooltipShown = value;
                
                tooltipBackground.transform.DOKill();
                
                if (!value)
                {
                    uiTooltip.text = "";
                    tooltipBackground.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                    {
                        tooltipBackground.SetActive(false);
                    });
                }
                else
                {
                    tooltipBackground.SetActive(true);
                    tooltipBackground.transform.DOScale(Vector3.one, 0.2f);
                }
            }
        }
        
        public void IconFocused(TooltipButton focusedButton)
        {
            IsTooltipShown = true;
            uiTooltip.text = focusedButton.ButtonInfo;
            focusedButton.IsHovered = true;
            
        }

        public void IconUnfocused(TooltipButton focusedObject)
        {
            IsTooltipShown = false;
            focusedObject.IsHovered = false;
        }

        void Update()
        {
            if (IsTooltipShown)
            {
                Vector2 mousePosition = Input.mousePosition;
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePosition,
                    canvas.worldCamera,
                    out Vector2 localPoint
                );
                
                tooltipBackground.transform.localPosition = new Vector2(localPoint.x + 150f, localPoint.y + 70);
            }
        }
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            swordSpriteBlock.TryGetComponent<Image>(out _swordImage);
        }

        
        private void OnEnable()
        {
            CurrentEnchantment = MagicEnchanterController.Instance.SwordEnchantment ? MagicEnchanterController.Instance.SwordEnchantment : null;
            PlacedSword = MagicEnchanterController.Instance.SwordToEnchant ? MagicEnchanterController.Instance.SwordToEnchant : null;
            
            _canvasGroup.blocksRaycasts = true;
            
            if (!CurrentEnchantment || !PlacedSword)
            {
                startEnchantmentButton.interactable = false;
            }
            else
            {
                startEnchantmentButton.interactable = true;
            }
            
            if (PlacedSword)
            {
                swordName.text = PlacedSword.SwordName;
                swordInfo.text = PlacedSword.SwordDescription;

                if (PlacedSword.IsEnchanted)
                {
                    enchantmentName.color = Color.green;
                    enchantmentName.text = _placedSword.SwordEnchantment.enchantmentName;

                    if (swordStats.Count == 5)
                    {
                        swordStats[0].text = _placedSword.PhysicalBonusDamageByAccuracy.ToString();
                        swordStats[1].text = _placedSword.PoisonBonusDamageByAccuracy.ToString();
                        swordStats[2].text = _placedSword.LightningBonusDamageByAccuracy.ToString();
                        swordStats[3].text = _placedSword.IceBonusDamageByAccuracy.ToString();
                        swordStats[4].text = _placedSword.FireBonusDamageByAccuracy.ToString();
                    }
                }
                else
                {
                    enchantmentName.color = Color.red;
                    enchantmentName.text = "Unset";
                    
                    foreach (var stat in swordStats)
                    {
                        stat.text = "0";
                    }
                }
            }
            else
            {
                swordName.text = "Empty";
                swordInfo.text = "No sword to analyze";
                _swordImage.sprite = undefinedSprite;
                
                enchantmentName.color = Color.red;
                enchantmentName.text = "No sword";
                    
                foreach (var stat in swordStats)
                {
                    stat.text = "-";
                }
            }
            

            if (CurrentEnchantment)
            {
                converterEnchantmentName.text = CurrentEnchantment.enchantmentName;
                converterEnchantmentName.color = Color.green;
            }
            else
            {
                converterEnchantmentName.text = "Unset";
                converterEnchantmentName.color = Color.red;
            }
        }

        public void CloseMenu()
        {
            rootSphereScript.Toggle();
            IsTooltipShown = false;
        }

        
        
        public void ConfirmEnchantment()
        {
            CloseMenu();
            drawPlace.SetActive(true);
        }

        
    }
}