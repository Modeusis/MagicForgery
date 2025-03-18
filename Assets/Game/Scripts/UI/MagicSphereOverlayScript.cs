using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MagicSphereOverlayScript : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button dismissEnchantmentButton;
        [SerializeField] private Button startEnchantmentButton;
        [SerializeField] private Image swordSprite;
        [SerializeField] private TMP_Text enchantmentInfo;
        [SerializeField] private TMP_Text swordInfo;
        
        
    }
}