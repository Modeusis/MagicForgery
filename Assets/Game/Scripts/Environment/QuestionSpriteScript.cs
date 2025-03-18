using System;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class QuestionSpriteScript : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Vector3 _spriteStartPosition;
        
        private bool _isVisible;
        private bool _isActive;

        private const string MoveSeqId = "moveQuestSeqId";

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value)
                    return;
                if (!_spriteRenderer)
                    return;
                
                _spriteRenderer.DOKill();
                DOTween.Kill(MoveSeqId);
                
                _isVisible = value;
                
                _spriteRenderer.DOFade(IsVisible ? 1f : 0f, 0.6f).OnComplete(() =>
                {
                    if (IsVisible)
                    {
                        IdleAnimation();
                    }
                    else
                    {
                        transform.localPosition = _spriteStartPosition;
                    }
                });
            }
        }
        
        void IdleAnimation()
        {
            var seq = DOTween.Sequence();
            
            seq.Append(transform.DOLocalMoveY(_spriteStartPosition.y + .2f, 0.6f));
            seq.Append(transform.DOLocalMoveY(_spriteStartPosition.y, 0.6f));

            seq.SetLoops(-1, LoopType.Restart);

            seq.SetId(MoveSeqId);
        }

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteStartPosition = transform.localPosition;
            
            IsVisible = false;
        }
    }
}