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

        public Arrow(Transform arrowTransform, float appearanceDuration, float disappearDuration, float timeToRotate)
        {
            _arrowTransform = arrowTransform;
            
            _appearanceDuration = appearanceDuration;
            _disappearDuration = disappearDuration;
            _rotateDuration = timeToRotate;
            
            _arrowTransform.localScale = Vector3.zero;
        }
        
        public void ShowArrow()
        {
            _arrowTransform.DOKill();
            
            _arrowTransform.DOLocalRotate(_rotationOnShow, _appearanceDuration);
            _arrowTransform.DOScale(Vector3.one, _appearanceDuration);
        }

        public void RotateArrowToTarget(Vector3 position)
        {
            _arrowTransform.DOKill();
            
            _arrowTransform.DOLookAt(position, _rotateDuration);
        }
        
        public void HideArrow()
        {
            _arrowTransform.DOKill();
            
            _arrowTransform.DOLocalRotate(_rotationOnShow, _appearanceDuration);
            _arrowTransform.DOScale(Vector3.one, _appearanceDuration);
        }

        public void LookAtTarget(Vector3 position)
        {
            _arrowTransform.DOKill();
            
            _arrowTransform.LookAt(position, Vector3.up);
        }

        public void ResetRotation()
        {
            _arrowTransform.DOKill();
            
            _arrowTransform.DOLocalRotate(Vector3.zero, 0.5f);
        }
    }
}