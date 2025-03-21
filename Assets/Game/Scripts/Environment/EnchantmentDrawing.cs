using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Environment
{
    public class EnchantmentDrawing : MonoBehaviour
    {
        //Optimization
        [SerializeField] private SpriteRenderer drawingArea;
        [SerializeField] private BoxCollider drawingCollider;
        [SerializeField] private Color brushColor = Color.black;
        [SerializeField] private int textureSize = 256;
        [SerializeField] private int brushSize = 5;
        
        private Texture2D _generatedTexture;
        private RectTransform _rectTransform;

        private bool _isDrawing;

        public bool IsDrawing
        {
            get => _isDrawing;
            set
            {
                if (_isDrawing == value)
                    return;
                _isDrawing = value;
                Debug.Log(value);
            }
        }

        private void OnEnable()
        {
            IsDrawing = true;
            
            _generatedTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
            _generatedTexture.filterMode = FilterMode.Point;
            
            ClearCanvas();

            drawingArea.sprite = TextureToSprite(_generatedTexture);
            drawingCollider.size = drawingArea.bounds.size;
            
            DrawCircle(128,128, brushSize, Color.black);
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
                clearPixels[i] = Color.white;
            }
            _generatedTexture.SetPixels(clearPixels);
            _generatedTexture.Apply();
        }
        
        private Sprite TextureToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }

        private void Draw()
        {
            Ray ray = Player.Player.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 4f))
            {
                if (hit.collider.gameObject == drawingArea.gameObject)
                {
                    Vector2 point = hit.point;
                    Vector2 localPoint = drawingArea.transform.InverseTransformPoint(hit.point);
                    
                    Bounds bounds = drawingArea.GetComponent<BoxCollider>().bounds;
                    
                    int x = (int)(localPoint.x * 100 + _generatedTexture.width / 2);
                    int y = (int)(localPoint.y * 100 + _generatedTexture.width / 2);
                    
                    Debug.Log($"adjusted point: {x}, {y}");
                    
                    DrawCircle(x, y, brushSize, brushColor);
                    
                    drawingArea.sprite = TextureToSprite(_generatedTexture);
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && IsDrawing)
            {
                Draw();
            }
        }
    }
}