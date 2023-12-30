using Characters.Base;
using Factory.Interface;
using GameConfig.LocalData;
using UnityEngine;

namespace Factory.Spawners
{
    public abstract class SpawnerBase : MonoBehaviour
    {
        public enum ExecutionType
        {
            Awake = 0,
            Start = 1,
            Manual = 2,
        }
        [InspectorName("Initialize On")] public ExecutionType executeOn = ExecutionType.Manual;
        private bool _isInitialized;
        
        protected virtual void Awake() => TryInitialize(ExecutionType.Awake);
        
        protected virtual void Start() => TryInitialize(ExecutionType.Start);
        
        public void TryInitialize(ExecutionType executionType)
        {
            if (executionType != executeOn)
            {
                return;
            }
            
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            Init();
        }
        
        protected virtual void Init()
        {

        }
    }
}