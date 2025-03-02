using System;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace UI
{
    public class MagicEngineController : MonoBehaviour
    {
        //Убрать изменение статуса напрямую, добавить метод который пытается включить и проверяет все состояния
        public static MagicEngineController Instance;
        
        [Header("Capacity")]
        [SerializeField] private int manaStorageCapacity;
        [SerializeField] private int waterStorageCapacity;
        
        [Header("GameObjects")]
        [SerializeField] private GameObject water;
        [SerializeField] private GameObject goldenLoop;
        [SerializeField] private GameObject goldenLoopMini;
        [SerializeField] private GameObject mainCrystal;

        [Header("Materials")] 
        [SerializeField] private Material waterFluid;
        [SerializeField] private Material manaFluid;
        [SerializeField] private Material magicFluid;
        
        [Header("Light Effects")]
        [SerializeField] private Light engineLightEffect;
        [SerializeField] private Light sphereLightEffect;
        [SerializeField] private Light sphereSecondLightEffect;
        
        private int _manaAmount;
        private int _waterAmount;
        private bool _isEngineWorking;

        private int ManaAmount
        {
            get => _manaAmount;
            set
            {
                if (value < 1)
                {
                    IsEngineWorking = false;
                    return;
                }
                _manaAmount = value;
            }
        }

        private int WaterAmount
        {
            get => _waterAmount;
            set
            {
                if (value < 1)
                {
                    IsEngineWorking = false;
                    return;
                }
                _waterAmount = value;
                
            }
        }

        public bool IsEngineWorking
        {
            get => _isEngineWorking;
            set
            {
                if (_isEngineWorking == value)
                    return;
                ToggleEngine();
                _isEngineWorking = value;
                
            }
        }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ToggleEngine()
        {
            goldenLoop.transform.DOKill();
            goldenLoopMini.transform.DOKill();
            mainCrystal.transform.DOKill();
            DOTween.Kill("StartEngineAnimation");
            DOTween.Kill("StartLightEngineAnimation");
            DOTween.Kill("TurnOffCrystalEngineAnimation");
            DOTween.Kill("TurnOffLoopEngineAnimation");
            DOTween.Kill("TurnOffMiniLoopEngineAnimation");
            DOTween.Kill("TurnOffLightEngineAnimation");
            
            if (IsEngineWorking)
            {
                TurnOffEngineAnimation();
            }
            else
            {
                StartEngineAnimation();
            }
        }

        void AddMana()
        {
            ManaAmount++;
        }

        void AddWater()
        {
            WaterAmount++;
        }

        void StartEngineAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            Sequence lightSequence = DOTween.Sequence();

            goldenLoop.transform.DOLocalMoveY(3f, 1.5f).SetEase(Ease.OutSine).OnComplete(() => InfiniteGoldenLoop());
            goldenLoopMini.transform.DOLocalMoveY(3f, 1f).SetEase(Ease.OutSine);
            
            engineLightEffect.enabled = true;
            sphereLightEffect.enabled = true;
            sphereSecondLightEffect.enabled = true;
            
            lightSequence.Append(engineLightEffect.DOIntensity(4f, 1f).SetEase(Ease.OutSine));
            lightSequence.Append(sphereLightEffect.DOIntensity(4f, .8f).SetEase(Ease.OutSine));
            lightSequence.Append(sphereSecondLightEffect.DOIntensity(4f, .8f).SetEase(Ease.OutSine));
            
            sequence.Append(mainCrystal.transform.DOLocalMoveY(3f, 1f).SetEase(Ease.OutSine));
            sequence.Append(mainCrystal.transform.DOScale(3f, .6f));
            
            sequence.OnComplete(() => InfiniteCrystalRotation());
            
            sequence.SetId("StartEngineAnimation");
            sequence.SetId("StartLightEngineAnimation");
        }
        
        void TurnOffEngineAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            Sequence loopSequence = DOTween.Sequence();
            Sequence miniLoopSequence = DOTween.Sequence();
            Sequence lightLoopSequence = DOTween.Sequence();
            
            loopSequence.Append(goldenLoop.transform.DOLocalRotate(Vector3.zero, 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutSine));
            loopSequence.Append(goldenLoop.transform.DOLocalMoveY(1.285f, 1f).SetEase(Ease.InSine));
            miniLoopSequence.Append(goldenLoopMini.transform.DOLocalRotate(Vector3.zero, .8f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutSine));
            miniLoopSequence.Append(goldenLoopMini.transform.DOLocalMoveY(1.295f, .8f).SetEase(Ease.InSine));
            sequence.Append(mainCrystal.transform.DOLocalRotate(Vector3.zero, .3f, RotateMode.FastBeyond360).SetEase(Ease.InSine));
            sequence.Append(mainCrystal.transform.DOScale(1f, .5f));
            sequence.Append(mainCrystal.transform.DOLocalMoveY(1.4f, 1.2f).SetEase(Ease.InSine));
            sequence.Append(engineLightEffect.DOIntensity(0f, 1f).SetEase(Ease.OutSine));

            lightLoopSequence.Append(sphereLightEffect.DOIntensity(0f, .4f).SetEase(Ease.OutSine));
            lightLoopSequence.Append(sphereSecondLightEffect.DOIntensity(0f, .4f).SetEase(Ease.OutSine));
            
            sequence.OnComplete(() =>
            {
                engineLightEffect.enabled = false;
                sphereLightEffect.enabled = false;
                sphereSecondLightEffect.enabled = false;
            });
            
            sequence.SetId("TurnOffCrystalEngineAnimation");
            sequence.SetId("TurnOffLoopEngineAnimation");
            sequence.SetId("TurnOffMiniLoopEngineAnimation");
            sequence.SetId("TurnOffLightEngineAnimation");
        }

        void InfiniteCrystalRotation()
        {
            mainCrystal.transform.DOLocalRotate(new Vector3(10f, 30f, -10f), 0.1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        void InfiniteGoldenLoop()
        {
            goldenLoop.transform.DOLocalRotate(new Vector3(0, 0, 30f), .6f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                goldenLoop.transform.DOLocalRotate(new Vector3(0, 360f, 0), 4f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Flash)
                    .SetLoops(-1, LoopType.Incremental);
            
                goldenLoopMini.transform.DOLocalRotate(new Vector3(0, -360f, 0), 3f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Flash)
                    .SetLoops(-1, LoopType.Incremental);
            });
            goldenLoopMini.transform.DOLocalRotate(new Vector3(0, 0, -10f), .5f).SetEase(Ease.OutSine);
        }
    }
}