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

        public Arrow(Transform arrowTransform, float appearanceDuration, float disappearDuration, float timeToRotate)
        {
            _arrowTransform = arrowTransform;
            
            _appearanceDuration = appearanceDuration;
            _disappearDuration = disappearDuration;
            _rotateDuration = timeToRotate;
            
            _arrowTransform.localScale = Vector3.zero;
        }
        
        public void LookAtTarget(Vector3 position)
        {
            //Remake to work only on Y axis
            
            _arrowTransform.LookAt(position, Vector3.up);
        }

        public void ShowArrow()
        {
            _arrowTransform.DOScale(Vector3.one, _appearanceDuration);
        }
        
        public void ResetRotation()
        {
            _arrowTransform.DOKill();
            
            _arrowTransform.DOLocalRotate(Vector3.zero, _rotateDuration);
            _arrowTransform.DOScale(Vector3.zero, _disappearDuration);
        }

        private void PlayEffect()
        {
            Debug.Log("on hide effect played");
        }
        

        public float GetDistanceToAim(Vector3 aimPosition)
        {
            return Vector3.Distance(_arrowTransform.position, aimPosition);
        }
    }
}