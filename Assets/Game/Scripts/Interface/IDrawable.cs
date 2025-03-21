using UnityEngine;

namespace Game.Scripts.Interface
{
    public interface IDrawable
    {
        public void Draw(RaycastHit hit);
    }
}