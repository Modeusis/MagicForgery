using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.MainMenu
{
    public class GameLogoScript : MonoBehaviour
    {
        [SerializeField] private float waveSpeed = 1f;        
        [SerializeField] private float waveFrequency = 1f;    
        [SerializeField] private float waveAmplitude = 75f;   
        [SerializeField] private float elementDelay = 0.2f;   
        [SerializeField] private List<RectTransform> logoElements;

        private List<Vector2> initialPositions = new List<Vector2>();

        private void Start()
        {
            foreach (var element in logoElements)
            {
                if (element != null)
                {
                    initialPositions.Add(element.anchoredPosition);
                }
            }
        }

        private void Update()
        {
            float baseTime = Time.time * waveSpeed;

            for (int i = 0; i < logoElements.Count; i++)
            {
                if (logoElements[i] == null) continue;
                
                float waveOffset = GetWaveOffset(i, baseTime);

                Vector2 newPos = new Vector2(
                    initialPositions[i].x,
                    initialPositions[i].y + waveOffset
                );
                
                logoElements[i].anchoredPosition = newPos;
            }
        }

        private float GetWaveOffset(int elementIndex, float baseTime)
        {
            float phase = baseTime - (elementIndex * elementDelay);
            
            return Mathf.Sin(phase * waveFrequency) * waveAmplitude;
        }
    }
}