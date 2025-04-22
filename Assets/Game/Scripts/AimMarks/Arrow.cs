using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.TargetMarks
{
    public class Arrow
    {
        private readonly float _appearanceDuration;
        private readonly float _disappearDuration;
        private readonly float _rotateDuration;
        
        private readonly Transform _arrowTransform;
        
        private Vector3 _rotationOnShow = new Vector3(0, 720, 0);

        private Tweener _currentScaleTween;

        private Tweener CurrentScaleTween
        {
            get => _currentScaleTween;
            set
            {
                _currentScaleTween = value;
                
                if (value != null)
                {
                    ToggleTweenOpenCheck();
                }
            }
        }
        
        private Tweener _currentRotateTween;
        
        private Tweener CurrentRotateTween
        {
            get => _currentRotateTween;
            set
            {
                _currentRotateTween = value;

                if (value != null)
                {
                    ToggleTweenOpenCheck();
                }
                
            }
        }

        public bool isArrowFree;

        public Arrow(Transform arrowTransform, float appearanceDuration, float disappearDuration, float timeToRotate)
        {
            _arrowTransform = arrowTransform;
            
            _appearanceDuration = appearanceDuration;
            _disappearDuration = disappearDuration;
            _rotateDuration = timeToRotate;
            
            _arrowTransform.localScale = Vector3.zero;
        }
        
        public void ShowArrow(Vector3 position)
        {
            KillTweens(); 
            
            CurrentRotateTween = _arrowTransform.DOLookAt(position, _appearanceDuration);
            CurrentScaleTween = _arrowTransform.DOScale(Vector3.one, _appearanceDuration);
        }
        
        public void HideArrow()
        {
            KillTweens();   
            
            CurrentRotateTween = _arrowTransform.DOLocalRotate(_rotationOnShow, _disappearDuration);
            CurrentScaleTween = _arrowTransform.DOScale(Vector3.one, _disappearDuration);
            
            CurrentRotateTween.OnComplete(PlayEffect);
        }

        public void LookAtTarget(Vector3 position)
        {
            _arrowTransform.LookAt(position, Vector3.up);
        }

        public void ResetRotation()
        {
            CurrentRotateTween.Kill();
            
            CurrentRotateTween = _arrowTransform.DOLocalRotate(Vector3.zero, 0.5f);
        }

        private void PlayEffect()
        {
            KillTweens();
            
            Debug.Log("on hide effect played");
        }

        private void KillTweens()
        {
            CurrentScaleTween.Kill();
            CurrentRotateTween.Kill();
        }

        private void ToggleTweenOpenCheck()
        {
            if (_currentScaleTween == null && _currentRotateTween == null)
            {
                isArrowFree = true;
                
                return;
            }

            if (_currentScaleTween == null || _currentRotateTween == null)
            {
                isArrowFree = false;
                
                return;
            }
            
            isArrowFree = true;
        }

        public float GetDistanceToAim(Vector3 aimPosition)
        {
            return Vector3.Distance(_arrowTransform.position, aimPosition);
        }
    }
}