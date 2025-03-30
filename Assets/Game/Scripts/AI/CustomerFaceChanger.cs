using UnityEngine;

namespace Game.Scripts.AI
{
    public class CustomerFaceChanger : MonoBehaviour
    {
        [SerializeField] private Texture2D _idleFace;
        [SerializeField] private Texture2D _angryFace;
        [SerializeField] private Texture2D _happyFace;
        
        [SerializeField] private Material _faceMaterial;

        public void SetAngryFace() => _faceMaterial.mainTexture = _angryFace;
        public void SetHappyFace() => _faceMaterial.mainTexture = _happyFace;
        public void SetIdleFace() => _faceMaterial.mainTexture = _idleFace;
    }
}