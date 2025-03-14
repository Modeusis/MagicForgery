using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class StaffScript : MonoBehaviour
    {
        [Header("Light")]
        [SerializeField] private GameObject magicCrystal;
        
        private void Update()
        {
            magicCrystal.transform.Rotate(Vector3.forward * (Time.deltaTime * 100));
        }
    }
}

