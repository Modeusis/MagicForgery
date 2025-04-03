using System;
using System.Collections;
using DG.Tweening;
using Game.Scripts.Interface;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Environment
{
    public class EnchantmentDrawing : MonoBehaviour, IDrawable
    {
        [Header("Drawing")]
        [SerializeField] private SpriteRenderer drawingArea;
        [SerializeField] private SpriteRenderer drawingMaskSpriteRenderer;
        [SerializeField] private Texture2D defaultTextureMask;
        [SerializeField] private BoxCollider drawingCollider;
        [SerializeField] private Color brushColor = Color.black;
        [SerializeField] private int textureSize = 256;
        [SerializeField] private int brushSize = 5;
        
        [Header("Timer")]
        [SerializeField] private int timerDuration = 5;
        [SerializeField] private TMP_Text timerValue;
        
        [Header("Size")]
        [SerializeField] private float drawingMaxScale = 0.6f;
        [SerializeField] private Vector3 drawingStartOffset = Vector3.zero;
        [SerializeField] private Vector3 drawingEndOffset;
        [SerializeField] private float translateDuration = 0.5f;
        
        private int _drawingCounter;
        
        private Texture2D _generatedTexture;
        private Enchantment _currentEnchantment;
        private RectTransform _rectTransform;
        private Texture2D _drawingMask;
        private Action _accuracySetAction;

        public float lastEnchantmentAccuracy;
        
        private Texture2D DrawingMask
        {
            get => _drawingMask;
            set
            {
                if (_drawingMask == value)
                    return;
                _drawingMask = value;
                if (_drawingMask)
                {
                    SetDrawingMask(_drawingMask, _currentEnchantment ? _currentEnchantment.enchantmentColor : Color.black);
                }
            }
        }
        
        private bool _isDrawing;

        public bool IsDrawing
        {
            get => _isDrawing;
            set
            {
                if (_isDrawing == value)
                    return;
                _isDrawing = value;
                _drawingCounter = 0;
            }
        }

        private void OnEnable()
        {
            _accuracySetAction += MagicEnchanterController.Instance.EnchantSword;
            
            var enchantment = MagicEnchanterController.Instance.SwordEnchantment;
            
            if (enchantment?.enchantmentMask)
            {
                _currentEnchantment = enchantment;
                DrawingMask = enchantment.enchantmentMask;
            }
            else
            {
                _currentEnchantment = null;
                DrawingMask = defaultTextureMask;
            }
            
            transform.localScale = Vector3.zero;
            
            StartCoroutine(ScaleDrawPlateOnStart(translateDuration));
            
            IsDrawing = true;
            
            _generatedTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
            _generatedTexture.filterMode = FilterMode.Point;
            
            ClearCanvas();

            drawingArea.sprite = TextureToSprite(_generatedTexture);
            drawingCollider.size = new Vector3((float)textureSize/100, (float)textureSize/100, 0);
            
            var coroutine = StartCoroutine(StartDrawingTimer(timerDuration, () =>
            {
                lastEnchantmentAccuracy = CompareMask();
                Debug.Log(lastEnchantmentAccuracy);
                _accuracySetAction?.Invoke();
                transform.DOLocalMove(drawingStartOffset, translateDuration);
                transform.DOScale(0, translateDuration)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
            }));
        }

        private void OnDisable()
        {
            _accuracySetAction -= MagicEnchanterController.Instance.EnchantSword;
            
            StopAllCoroutines();
            IsDrawing = false;
        }
        
        void DrawCircle(float x, float y, int radius, Color color)
        {
            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    if (i * i + j * j <= radius * radius)
                    {
                        int texX = (int)x + i;
                        int texY = (int)y + j;
                        
                        if (texX >= 0 && texX < _generatedTexture.width && texY >= 0 && texY < _generatedTexture.height)
                        {
                            _generatedTexture.SetPixel(texX, texY, color);
                        }
                    }
                }
            }
            _generatedTexture.Apply();
        }

        void ClearCanvas()
        {
            Color[] clearPixels = new Color[_generatedTexture.width * _generatedTexture.height];
            for (int i = 0; i < clearPixels.Length; i++)
            {
                clearPixels[i] = Color.clear;
            }
            _generatedTexture.SetPixels(clearPixels);
            _generatedTexture.Apply();
        }
        
        private Sprite TextureToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        
        public void Draw(RaycastHit hit)
        {
            _drawingCounter++;
            
            if (_drawingCounter == 3)
            {
                _drawingCounter = 0;
                
                if (hit.collider == drawingCollider)
                {
                    Vector2 localPoint = drawingArea.transform.InverseTransformPoint(hit.point);
                
                    int x = (int)(localPoint.x * 100 + _generatedTexture.width / 2);
                    int y = (int)(localPoint.y * 100 + _generatedTexture.width / 2);
                
                    DrawCircle(x, y, brushSize, brushColor);
                
                    drawingArea.sprite = TextureToSprite(_generatedTexture);
                }
            }
        }
        
        private IEnumerator StartDrawingTimer(int time, Action callback)
        {
            var timer = time;

            while (timer > 0)
            {
                timerValue.text = timer.ToString();
                timerValue.transform.DOScale(2, .2f).OnComplete(() =>
                {
                    timerValue.transform.DOScale(1, .2f);
                });
                
                timer--;
                yield return new WaitForSeconds(1);
            }
            
            callback?.Invoke();
        }

        private IEnumerator ScaleDrawPlateOnStart(float duration)
        {
            transform.DOKill();
            transform.DOLocalMove(drawingEndOffset, duration);
            transform.DOScale(drawingMaxScale, duration);
            yield return new WaitForSeconds(duration);
        }

        private float CompareMask()
        {
            if (!DrawingMask)
                return 0f;

            float paintedPixelsCount = 0;
            float drawingMaxPixels = 0;
            
            Color[] paintedPixels = _generatedTexture.GetPixels();
            Color[] maskPixels = DrawingMask.GetPixels();
            
            for (int i = 0; i < paintedPixels.Length; i++)
            {
                if (maskPixels[i] == Color.clear)
                    continue;
                
                drawingMaxPixels++;
                
                if (paintedPixels[i] == maskPixels[i])
                {
                    paintedPixelsCount++;
                }
            }
            
            return paintedPixelsCount / drawingMaxPixels;
        }
        
        private void SetDrawingMask(Texture2D drawingMaskSprite, Color drawingColor)
        {
            if (!drawingMaskSpriteRenderer)
                return;
            
            brushColor = drawingColor;
            
            if (brushColor != Color.black)
            {
                Color[] maskPixels = drawingMaskSprite.GetPixels();

                for (int i = 0; i < maskPixels.Length; i++)
                {
                    if (maskPixels[i] == Color.clear)
                        continue;
                
                    maskPixels[i] = drawingColor;
                }
            
                drawingMaskSprite.SetPixels(maskPixels);
                drawingMaskSprite.Apply();
            }
            
            var spriteMask = TextureToSprite(drawingMaskSprite);
            
            drawingMaskSpriteRenderer.sprite = spriteMask;
        }
    }
}