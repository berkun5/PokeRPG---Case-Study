using System.Collections;
using DataManagement;
using GameConfig.Enum;
using Managers.Base;
using UnityEngine;

namespace Managers
{
    public class SettingsManager : ManagerBase
    {
        private const float AutoSaveTickTimeInSeconds = 10;
        
        public LocalDataCollection LocalData => localData;
        public RemoteDataProvider RemoteData { get; private set; }

        [SerializeField] private LocalDataCollection localData;
        private bool _autosaveStarted;
        
        public override void Init()
        {
            RemoteData = new RemoteDataProvider(localData.CharacterConfigs);
        }

        public override void LateStart() => StartCoroutine(SaveOverTime());

        private void OnDestroy()
        {
            StopAllCoroutines();
            RemoteData.SaveAllData();
        }

        private void OnApplicationQuit()=> RemoteData.SaveAllData();
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                RemoteData.SaveAllData();
            }
        }

        private IEnumerator SaveOverTime()
        {
            if (_autosaveStarted)
            {
                yield break;
            }
            _autosaveStarted = true; 
            
            var autoSaveTick = new WaitForSeconds(AutoSaveTickTimeInSeconds);
            while (true)
            {
                yield return autoSaveTick;
                if (RemoteData.GameData.GetGameState() == GameState.Combat)
                {
                    RemoteData?.SaveAllData();
                }
            }
        }
        
    }
}
