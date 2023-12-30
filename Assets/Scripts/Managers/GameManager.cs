using GameConfig.Enum;
using GameConfig.RemoteData;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers.Base;
using Managers.UI;
using UI.CharacterSelection;

namespace Managers
{
    public class GameManager : ManagerBase
    {
        private UIModelManager _uiModel;
        private SettingsManager _settingsManager;
        
        public override void Init()
        {
          //do nothing atm..
        }

        public override void LateStart()
        {
            _uiModel = GameServiceLocator.GetService<UIModelServiceProvider>()
                .GetManager<UIModelManager>();

            _settingsManager = GameServiceLocator.GetService<PersistentServiceProvider>()
                .GetManager<SettingsManager>();
            
            StartGame();
        }

        private void StartGame()
        {
            var gameData = _settingsManager.RemoteData.GameData;
            
            switch (gameData.GetGameState())
            {
                default:
                case GameState.CharacterSelection:
                    BeginCharacterSelection(gameData);
                    gameData.SetCombatState(CombatState.PlayerTurn);
                    break;
                case GameState.Combat:
                    StartCombat();
                    break;
            }
        }
        
        private void BeginCharacterSelection(GameData gameData)
        {
            var localCharacterConfig = _settingsManager.LocalData.CharacterConfigs;
            
            _uiModel.Show<CharacterSelectionWindow>(window => 
                window.Init(new CharacterSelectionWindowViewModel(localCharacterConfig, gameData.GetUnlockedCharacterCount())));
        }
        
        private void StartCombat()
        {
            DevLog.Log(_settingsManager.RemoteData.GameData.GetCombatState());
            
            GameServiceLocator.GetService<CombatServiceProvider>()
                .GetManager<CombatManager>().StartCombat();
        }
    }
}
