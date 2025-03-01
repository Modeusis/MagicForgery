using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class MagicCnverterScript : MonoBehaviour
    {
        [SerializeField] private GameObject magicConverterHead;
        [SerializeField] private GameObject magicCrystal;

        private bool _isMagicConverterWorking;

        private bool IsMagicConverterWorking
        {
            get => _isMagicConverterWorking;
            set
            {
                if (_isMagicConverterWorking == value)
                    return;
                _isMagicConverterWorking = value;
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