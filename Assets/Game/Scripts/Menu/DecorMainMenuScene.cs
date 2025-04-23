using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.MainMenu
{
    public class DecorMainMenuScene : MonoBehaviour
    {
        [Header("Animation delay range")]
        [SerializeField] private Vector2 animationDelayRange = new Vector2(12f, 20f);
        
        [SerializeField] private GameObject magicCrystal;
        [SerializeField] private GameObject magicSphere;
        
        [SerializeField] private GameObject butterfly;
        
        [SerializeField] private Animator featherAnimator;
        
        private Tweener _sphereMoveTween;
        private Tweener _sphereTween;
        
        private Tweener _crystalMoveTween;
        private Tweener _crystalTween;
        
        private Tweener _butterflyTween;
        
        private Coroutine _featherCoroutine;
        private void Start()
        {
            _crystalTween = magicCrystal.transform.DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);

            _crystalMoveTween = magicCrystal.transform.DOLocalMoveY(0.78f, 5)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
            
            _sphereTween = magicSphere.transform.DORotate(new Vector3(0f, 360f, 0f), 8f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            _sphereMoveTween = magicSphere.transform.DOLocalMoveY(2.2f, 8)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            if (_featherCoroutine == null)
            {
                FeatherAnimation(Random.Range(animationDelayRange.x, animationDelayRange.y));
            }
        }
        
        private void OnDisable()
        {
            _crystalTween?.Kill();
            _crystalMoveTween?.Kill();
            
            _sphereTween?.Kill();
            _sphereMoveTween?.Kill();
            
            if (_featherCoroutine != null)
            {
                StopCoroutine(_featherCoroutine);
                
                _featherCoroutine = null;
            }
        }

        private void FeatherAnimation(float delay)
        {
            if (_featherCoroutine != null)
            {
                StopCoroutine(_featherCoroutine);
                
                _featherCoroutine = null;
            }
            
            _featherCoroutine = StartCoroutine(FeatherCoroutine(delay, () =>
            {
                _featherCoroutine = null;
            }));
        }
        
        private IEnumerator FeatherCoroutine(float delay, Action callback = null)
        {
            yield return new WaitForSeconds(delay);
            
            featherAnimator.SetTrigger("WriteDownTrigger");
            
            callback?.Invoke();
        }
    }
}