using System.Collections;
using DG.Tweening;
using Game.Scripts.Interface;
using UI;
using UnityEngine;

namespace Environment
{
    public class MagicBookScript : MonoBehaviour, IToggle
    {
        [SerializeField] private Animator bookAnimator;
        [SerializeField] private GameObject bookMesh;
        [SerializeField] private GameObject bookCanvas;
        
        private bool _isFocused;
        private bool _isToggled;
        
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused == value)
                    return;
                _isFocused = value;
                bookMesh.layer = value ? LayerMask.NameToLayer("Interactable") : LayerMask.NameToLayer("Default");
                TooltipController.Instance.TooltipMessage = $"{Player.Player.instance.InteractKey} to open guide";
                TooltipController.Instance.IsTooltipShowed = value;
            }
        }

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                if (_isToggled == value)
                    return;
                _isToggled = value;
                AnimateBookOpening();
            }
        }
        public void Toggle()
        {
            IsToggled = !IsToggled;
        }

        void Awake()
        {
            bookAnimator = GetComponent<Animator>();
        }
        
        void AnimateBookOpening()
        {
            if (!bookAnimator)
                return;
            
            transform.DOKill();
            
            if (IsToggled)
            {
                transform.DOLocalMove(new Vector3(0.05f, 1.9f, 0.407f), 0.5f).SetEase(Ease.OutSine);
                transform.DOLocalRotate(new Vector3(1.26023912f,90.8825989f,305.009705f), 0.5f).SetEase(Ease.OutSine).OnComplete(
                    () =>
                    {
                        bookAnimator.SetBool("IsBookOpened", IsToggled);
                    });
            }
            else
            {
                bookAnimator.SetBool("IsBookOpened", IsToggled);
                transform.DOLocalMove(new Vector3(0.25f, 1.4f, 0.407f), 0.5f).SetEase(Ease.InSine).SetDelay(bookAnimator.GetCurrentAnimatorStateInfo(0).length);
                transform.DOLocalRotate(new Vector3(305f, 0, 0), 0.5f).SetEase(Ease.InSine).SetDelay(bookAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
        }

        IEnumerator AnimatorAwaitCoroutine(Animator animator)
        {
            animator.SetBool("IsBookOpened", IsToggled);
            
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}