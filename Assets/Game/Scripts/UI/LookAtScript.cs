using UnityEngine;

namespace UI
{
    public class LookAtScript : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        void Update()
        {
            if (cameraTransform)
            {
                transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                    cameraTransform.rotation * Vector3.up);
            }
        }
    }
}