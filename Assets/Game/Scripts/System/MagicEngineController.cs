using System;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace UI
{
    public class MagicEngineController : MonoBehaviour
    {
        public static MagicEngineController Instance;
        
        [Header("Capacity")]
        [SerializeField] private int manaStorageCapacity;
        [SerializeField] private int waterStorageCapacity;
        
        [Header("GameObjects")]
        [SerializeField] private GameObject water;
        [SerializeField] private GameObject goldenLoop;
        [SerializeField] private GameObject mainCrystal;
        [SerializeField] private GameObject lever;

        [Header("Materials")] 
        [SerializeField] private Material waterFluid;
        [SerializeField] private Material manaFluid;
        [SerializeField] private Material magicFluid;
        
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

        private bool IsEngineWorking
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

        void Update()
        {
            if (Player.Player.instance.IsPlayerEnabled)
            {
                if (Input.GetKeyDown(KeyCode.K))
                {
                    IsEngineWorking = !IsEngineWorking;
                }
            }
        }

        void ToggleEngine()
        {
            goldenLoop.transform.DOKill();
            mainCrystal.transform.DOKill();
            DOTween.Kill("StartEngineAnimation");
            DOTween.Kill("TurnOffEngineAnimation");
            
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
            
            sequence.Append(mainCrystal.transform.DOLocalMoveY(3f, 1f).SetEase(Ease.OutSine));
            sequence.Append(mainCrystal.transform.DOScale(3f, .6f));
            
            sequence.OnComplete(() => InfiniteCrystalRotation());
            
            sequence.SetId("StartEngineAnimation");
        }
        
        void TurnOffEngineAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(mainCrystal.transform.DORotate(Vector3.zero, 1f, RotateMode.FastBeyond360).SetEase(Ease.OutSine));
            sequence.Append(mainCrystal.transform.DOScale(1f, 1f));
            sequence.Append(mainCrystal.transform.DOLocalMoveY(1.4f, 2f).SetEase(Ease.OutSine));
            
            sequence.SetId("TurnOffEngineAnimation");
        }

        void InfiniteCrystalRotation()
        {
            mainCrystal.transform.rotation = Quaternion.identity;
            
            mainCrystal.transform.DORotate(new Vector3(10f, 30f, -10f), 0.1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }
    }
}