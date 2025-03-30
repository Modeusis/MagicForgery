using UnityEngine;

namespace Game.Scripts.Interface
{
    public interface IDrawable
    {
        public bool IsDrawing {get; set; }
        public void Draw(RaycastHit hit);
    }
}