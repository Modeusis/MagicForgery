using Environment.Hydromat;
using Game.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment
{
    public class GenerateBucketButton : ActionButton
    {
        [SerializeField] private BucketGeneratorScript bucketGeneratorScript;
        
        private void OnEnable()
        {
            SetButtonColor(true);
            OnPressed += bucketGeneratorScript.SpawnBucket;
            bucketGeneratorScript.IsSpawnAvailable += SetButtonColor;
        }

        private void OnDisable()
        {
            OnPressed -= bucketGeneratorScript.SpawnBucket;
            bucketGeneratorScript.IsSpawnAvailable -= SetButtonColor;
        }
    }
}