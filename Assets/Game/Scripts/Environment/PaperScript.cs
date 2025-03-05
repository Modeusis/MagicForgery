using System;
using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class PaperScript : MonoBehaviour
    {
        [SerializeField] private Animator featherAnimator;

        private Material featherMaterial;
        void Awake()
        {
            featherMaterial = GetComponent<Renderer>().material;
        }
        
        void WriteDown()
        {
            featherMaterial.DOKill();
            
            featherAnimator.SetTrigger("WriteDownTrigger");
            featherMaterial.DOFloat(1.2f, "_BlendValue", 6f).SetEase(Ease.InQuad).SetDelay(0.5f);
        }

        void EraseText()
        {
            featherMaterial.DOKill();
            
            featherMaterial.DOFloat(-0.2f, "_BlendValue", 0.8f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                WriteDown();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                EraseText();
            }
        }
    }
}