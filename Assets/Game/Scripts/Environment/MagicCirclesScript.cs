using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class MagicCirclesScript : MonoBehaviour
    {
        [SerializeField] private Transform smallMagicCircle;
        [SerializeField] private Transform normalMagicCircle;
        [SerializeField] private Transform symbolsCircle;
        
        [SerializeField] private float smallRotateSpeed = 20f;
        [SerializeField] private float normalRotateSpeed = -20f;
        [SerializeField] private float symbolRotateSpeed = 10f;
        [SerializeField] private float smallRotateTime = 1f;
        [SerializeField] private float normalRotateTime = 1f;
        [SerializeField] private float symbolRotateTime = 1f;
        
        private Tween _smallRotateTween;
        private Tween _symbolRotateTween;
        private Tween _normalRotateTween;
        
        private void OnEnable()
        {
            smallMagicCircle.localRotation = Quaternion.Euler(0,0,0);
            normalMagicCircle.localRotation = Quaternion.Euler(0,0,0);
            symbolsCircle.localRotation = Quaternion.Euler(0,0,0);

            _smallRotateTween = smallMagicCircle.DOLocalRotate(new Vector3(0, 0, smallRotateSpeed), smallRotateTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
            _normalRotateTween = normalMagicCircle.DOLocalRotate(new Vector3(0, 0, normalRotateSpeed), normalRotateTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
            _symbolRotateTween = symbolsCircle.DOLocalRotate(new Vector3(0, 0, symbolRotateSpeed), symbolRotateTime, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        private void OnDisable()
        {
            _smallRotateTween?.Kill();
            _normalRotateTween?.Kill();
            _symbolRotateTween?.Kill();
        }
    }
}