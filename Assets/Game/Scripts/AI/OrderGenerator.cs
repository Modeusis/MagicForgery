using System;
using System.Collections;
using Environment;
using Game.Scripts.AI.CustomerStateMachine;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.Scripts.AI
{
    public class OrderGenerator : IDisposable
    {
        private TMP_Text _enchantmentName;
        private TMP_Text _enchantmentRequiredAccuracy;
        private Image _enchantmentProgressBar;
        private CanvasGroup _orderCanvasGroup;
        
        private EnchantmentData _availableEnchantments;
        private Vector2 _timeRange;
        private Vector2 _accuracyRange;
        
        private float _swordScaleOnPlace;
        private GameObject _swordPrefab;
        private Transform _swordParent;
        private Vector3 _positionOnPlace;
        private Vector3 _rotationOnPlace;
        
        private int _playersToWin = 5;
        private TMP_Text _clientCounter;
        
        private float _accuracyMinimum;
        private float _timerValue;
        
        private Enchantment _orderEnchantment;
        private GameFinishCounter _gameFinishCounter;
        private Coroutine _timerCoroutine;

        public CustomerFaceChanger faceChanger;
        
        private MonoBehaviour _coroutineRunner;
        
        private bool _isFinished;

        public OrderGenerator(
            MonoBehaviour coroutineRunner,
            
            TMP_Text enchantmentName,
            TMP_Text enchantmentRequiredAccuracy,
            Image enchantmentProgressBar,
            CanvasGroup orderCanvasGroup,
            TMP_Text clientCounter,

            EnchantmentData availableEnchantments,

            GameObject swordPrefab,
            Transform swordParent,
            float swordScaleOnPlace,
            Vector3 positionOnPlace,
            Vector3 rotationOnPlace,

            Vector2 timeRange,
            Vector2 accuracyRange,
            int playersToWin)
        {
            _coroutineRunner = coroutineRunner;
            
            _enchantmentName = enchantmentName;
            _enchantmentRequiredAccuracy = enchantmentRequiredAccuracy;
            _enchantmentProgressBar = enchantmentProgressBar;
            _orderCanvasGroup = orderCanvasGroup;
            _clientCounter = clientCounter;

            _availableEnchantments = availableEnchantments;

            _swordPrefab = swordPrefab;
            _swordParent = swordParent;
            _swordScaleOnPlace = swordScaleOnPlace;
            _positionOnPlace = positionOnPlace;
            _rotationOnPlace = rotationOnPlace;

            _timeRange = timeRange;
            _accuracyRange = accuracyRange;
            _accuracyMinimum = accuracyRange.x;
            _playersToWin = playersToWin;
            
            _gameFinishCounter = new GameFinishCounter(_playersToWin, _clientCounter);
        }

        public bool IsOrderFinished()
        {
            if (_isFinished)
            {
                _isFinished = false;
                
                return true;
            }
            
            return false;
        }

        public Vector3 GetOrderPosition()
        {
            return _orderCanvasGroup.transform.position;
        }
        
        public void StartOrder()
        {
            if (GenerateOrder())
            {
                _enchantmentName.text = _orderEnchantment.name;
                _enchantmentRequiredAccuracy.text = _accuracyMinimum.ToString();
                
                _coroutineRunner.StartCoroutine(SetCanvasAlpha(1f));
                
                if (GenerateSword())
                {
                    _timerCoroutine = _coroutineRunner.StartCoroutine(TimerCoroutine(_timerValue, CompleteOrder));
                }
            }
        }

        private bool GenerateOrder()
        {
            var enchantmentList = _availableEnchantments.Enchantments;
            int enchantmentCount = enchantmentList.Count;
            
            if (enchantmentCount <= 0)
            {
                Debug.LogWarning("No Enchantment available");
                
                return false;
            }
            
            var enchantmentId = Random.Range(0, enchantmentCount);
            
            _orderEnchantment = enchantmentList[enchantmentId].Enchantment;
            _accuracyMinimum = Mathf.Round(Random.Range(_accuracyRange.x, _accuracyRange.y));
            _timerValue = Random.Range(_timeRange.x, _timeRange.y);

            return true;
        }

        private bool ValidateOrder(Sword enchantedSword)
        {
            if (!enchantedSword.IsEnchanted)
            {
                TooltipController.Instance.ShowMechanicsDescription("Sword is not enchanted");
                
                return false;   
            }
            if (enchantedSword.SwordEnchantment != _orderEnchantment)
            {
                TooltipController.Instance.ShowMechanicsDescription("Wrong enchantment");
                return false;
            }

            if (enchantedSword.EnchantmentAccuracy < _accuracyMinimum / 100)
            {
                TooltipController.Instance.ShowMechanicsDescription("Bad enchantment");
                return false;
            }
            return true;
        }

        private void ResetOrderGUI()
        {
            _orderEnchantment = null;
            _accuracyMinimum = 0;
            _timerValue = 0;
            
            _enchantmentName.text = "";
            _enchantmentRequiredAccuracy.text = "";
            
            _coroutineRunner.StartCoroutine(SetCanvasAlpha(1f, false));
        }

        public void SkipOrder()
        {
            if (_timerCoroutine == null)
            {
                return;
            }
            
            CompleteOrder(false);
            ResetOrderGUI();
        }

        public void FinishOrder()
        {
            var selectedItem = Player.Player.instance.selectedItem;
            if (!selectedItem)
                return;
            
            selectedItem.prefab.TryGetComponent(out Sword selectedSword);
            
            if (ValidateOrder(selectedSword))
            {
                Inventory.instance.RemoveItem();
                CompleteOrder(true);
            }
            else
            {
                faceChanger?.SetAngryFace();
            }
        }
        
        private void CompleteOrder(bool orderStatus)
        {
            if (orderStatus)
            {
                faceChanger?.SetHappyFace();
                _gameFinishCounter.ClientSuccess();
            }
            else
            {
                faceChanger?.SetAngryFace();
                _gameFinishCounter.ClientExpired();
            }
            
            _isFinished = true;
            
            if (_timerCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_timerCoroutine);
                
                _timerCoroutine = null;
            }
            
            ResetOrderGUI();
        }

        private bool GenerateSword()
        {
            if (!_swordPrefab || !_swordParent)
            {
                return false;
            }
            
            var sword = Object.Instantiate(_swordPrefab, _swordParent);
            sword.transform.localScale *= _swordScaleOnPlace;
            sword.transform.localPosition = _positionOnPlace;
            sword.transform.localRotation = Quaternion.Euler(_rotationOnPlace);

            return true;
        }
        
        private IEnumerator TimerCoroutine(float time, Action<bool> callback = null)
        {
            var timer = 0f;
            while (timer < time)
            {
                var t = timer / time;
                _enchantmentProgressBar.fillAmount = 1 - t;
                timer += Time.deltaTime;
                yield return null;
            }

            _enchantmentProgressBar.fillAmount = 0f;
            
            callback?.Invoke(false);
        }

        private IEnumerator SetCanvasAlpha(float duration, bool visible = true)
        {
            var timer = 0f;
            while (timer < duration)
            {
                var t = timer / duration;
                _orderCanvasGroup.alpha = visible ? t : 1 - t;
                timer += Time.deltaTime;
                yield return null;
            }
        }

        public void Dispose()
        {
            
        }
    }
}