using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace Game.Scripts.MiniActivities
{
    public class WaterClaimingScript : MonoBehaviour
    {
        [Header("Mini Activities UI")]
        [SerializeField] private Image progressBarBorder;
        [SerializeField] private Image progressBar;
        [SerializeField] private Image interactKeyImage;
        [SerializeField] private TMP_Text interactKeyValue;
        [SerializeField] private Image interactKeyImageEffect;
        [SerializeField] private int maxProgress = 100;
        [SerializeField] private int progressStep = 2;
        [SerializeField] private int startProgress = 20;
        
        [Header("Item")]
        [SerializeField] private ItemData filledWater;
        
        private int _currentProgress;

        private int CurrentProgress
        {
            get => _currentProgress;
            set
            {
                if (_currentProgress == value)
                    return;
                if (value >= maxProgress)
                {
                    _currentProgress = maxProgress;
                    progressBar.fillAmount = 1;
                    IsEmptyBucketSelected = false;
                    FillBucket();
                    return;
                }
                if (value <= 0)
                {
                    _currentProgress = 0;
                    progressBar.fillAmount = 0;
                    return;
                }    
                _currentProgress = value;
                progressBar.fillAmount = (float)_currentProgress/maxProgress;
                
            }
        }
        
        private bool _isInDrawWellZone;

        private bool IsInDrawWellZone
        {
            get => _isInDrawWellZone;
            set
            {
                if (_isInDrawWellZone == value)
                    return;
                _isInDrawWellZone = value;
                TooltipController.Instance.IsTooltipShowed = _isInDrawWellZone;
                TooltipController.Instance.TooltipMessage = $"Press {Player.Player.instance.InteractKey} to fill bucket";
            }
        }

        private bool _isEmptyBucketSelected;

        private bool IsEmptyBucketSelected
        {
            get => _isEmptyBucketSelected;
            set
            {
                if (_isEmptyBucketSelected == value)
                    return;
                _isEmptyBucketSelected = value;
                Player.Player.instance.IsMiniGamePlayed = _isEmptyBucketSelected;
                StopCoroutine(DecreaseProgressOverTime());
                
                if (value)
                {
                    CurrentProgress = startProgress;
                    StartCoroutine(DecreaseProgressOverTime());
                }
                
                progressBarBorder.DOKill();
                progressBar.DOKill();
                interactKeyImage.DOKill();
                interactKeyImageEffect.DOKill();
                interactKeyImageEffect.transform.DOKill();
                DOTween.Kill("QTEEffectLoop");
                
                progressBarBorder.DOFade(value ? 1f : 0f, 0.4f);
                progressBar.DOFade(value ? 1f : 0f, 0.4f);
                interactKeyImage.DOFade(value ? 1f : 0f, 0.5f);
                interactKeyValue.DOFade(value ? 1f : 0f, 0.5f);
                interactKeyImageEffect.DOFade(value ? 1f : 0f, 0.5f).OnComplete(() =>
                {
                    if (value)
                    {
                        Sequence seq = DOTween.Sequence();
                        
                        seq.Append(interactKeyImageEffect.transform.DOScale(1.1f, 0.5f));
                        seq.Append(interactKeyImageEffect.transform.DOScale(0.9f, 0.5f));
                        
                        seq.SetLoops(-1, LoopType.Restart);
                        seq.SetId("QTEEffectLoop");
                    }
                });
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsInDrawWellZone = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsInDrawWellZone = false;
                IsEmptyBucketSelected = false;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(Player.Player.instance.BreakKey) && Player.Player.instance.IsMiniGamePlayed && IsEmptyBucketSelected)
            {
                IsEmptyBucketSelected = false;
                return;
            }
            if (IsEmptyBucketSelected)
            {
                if (Input.GetKeyDown(Player.Player.instance.InteractKey))
                {
                    interactKeyImage.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    CurrentProgress += progressStep;
                }
                else if (Input.GetKeyUp(Player.Player.instance.InteractKey))
                {
                    interactKeyImage.transform.localScale = Vector3.one;
                }
                return;
            }
            if (IsInDrawWellZone && Input.GetKeyDown(Player.Player.instance.InteractKey))
            {
                if (Player.Player.instance.selectedItem)
                {
                    if (Player.Player.instance.selectedItem.itemName == "Bucket")
                    {
                        IsEmptyBucketSelected = true;
                        IsInDrawWellZone = false;
                    }
                    else
                    {
                        TooltipController.Instance.ShowMechanicsDescription("Select empty bucket first");
                    }
                }
                else
                {
                    TooltipController.Instance.ShowMechanicsDescription("Select empty bucket first");
                }
            }
        }

        void FillBucket()
        {
            Inventory.instance.RemoveItem();

            if (filledWater.prefab)
            {
                Inventory.instance.AddItem(filledWater);
            }
        }
        
        private IEnumerator DecreaseProgressOverTime()
        {
            while (IsEmptyBucketSelected)
            {
                yield return new WaitForSeconds(0.2f);
                CurrentProgress -= 1;
            }
        }
    }
}