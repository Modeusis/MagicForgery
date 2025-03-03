using DG.Tweening;
using UI;
using UnityEngine;

namespace Environment
{
    public class MagicConverterScript : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject magicConverterHead;
        [SerializeField] private GameObject magicCrystal;
        
        [Header("Keycodes")]
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        
        private bool _isMagicConverterInFocus;

        private bool IsMagicConverterInFocus
        {
            get => _isMagicConverterInFocus;
            set
            {
                if (_isMagicConverterInFocus == value)
                    return;
                _isMagicConverterInFocus = value;
                TooltipController.Instance.TooltipMessage = $"Press {interactKey} to {(IsMagicConverterWorking ? "stop" : "start")} magic converter";
                TooltipController.Instance.IsTooltipShowed = value;
                gameObject.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                magicConverterHead.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                magicCrystal.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
            }
        }
        
        private bool _isMagicConverterWorking;

        private bool IsMagicConverterWorking
        {
            get => _isMagicConverterWorking;
            set
            {
                if (_isMagicConverterWorking == value)
                    return;
                _isMagicConverterWorking = value;
                TooltipController.Instance.TooltipMessage = $"Press {interactKey} to {(IsMagicConverterWorking ? "stop" : "start")} magic converter";
                AnimateMagicConverter();
            }
        }

        void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3f) && hit.collider.CompareTag("MagicConverter"))
                {
                    IsMagicConverterInFocus = true;
                    if (Input.GetKeyDown(interactKey))
                    {
                        if (MagicEngineController.Instance.IsEngineWorking)
                        {
                            IsMagicConverterWorking = !IsMagicConverterWorking;
                        }
                        else
                        {
                            //Добавить анимацию не удачного запуска
                            TooltipController.Instance.ShowMechanicsDescription("Turn engine first");
                        }
                    }
                }
                else
                {
                    IsMagicConverterInFocus = false;
                }
            }
        }
        
        void AnimateMagicConverter()
        {
            magicConverterHead.transform.DOKill();
            magicCrystal.transform.DOKill();
            
            if (IsMagicConverterWorking)
            {
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 1f, 0), 3f);
                
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.7f, 0), 3f);
                magicCrystal.transform.DOLocalRotate(new Vector3(0, 360f, 0), 3f, RotateMode.FastBeyond360);
                
            }
            else
            {
                
                magicConverterHead.transform.DOLocalRotate(new Vector3(0, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicConverterHead.transform.DOLocalMove(new Vector3(0, 0.6f, 0), 3f);
                
                magicCrystal.transform.DOLocalRotate(new Vector3(90f, -360f, 0), 3f, RotateMode.FastBeyond360);
                magicCrystal.transform.DOLocalMove(new Vector3(0, 0.5f, 0), 3f);
                

            }
        }
    }
}