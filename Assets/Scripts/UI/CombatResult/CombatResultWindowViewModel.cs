using System.Collections.Generic;
using GameConfig.Enum;
using GameConfig.LocalData;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers;
using Managers.UI;
using UI.Character;
using UnityEngine;

namespace UI.CombatResult
{
    public class CombatResultWindowViewModel : ICombatResultWindowViewModel
    {
        public string ResultHeader { get; }
        public List<ICharacterViewModel> SelectedCharacterViewModels { get; } = new();
        
        private readonly UIModelManager _uiModelManager;
        private readonly LevelManager _levelManager;
        
        public CombatResultWindowViewModel(UIModelManager uiModelManager,SettingsManager settingsManager,
            List<CharacterId> selectedCharacters, bool win)
        {
            _levelManager = GameServiceLocator.GetService<PersistentServiceProvider>().GetManager<LevelManager>();
            _uiModelManager = uiModelManager;
            ResultHeader = GetResultHeader(win);
            settingsManager.RemoteData.GameData.SetGameState(GameState.CharacterSelection);
            InitCharacterViewModels(settingsManager.LocalData.CharacterConfigs, selectedCharacters, win);
            DevLog.Log($"Player was successful: {win}.");
        }
        
        private void InitCharacterViewModels(CharacterConfigs characterConfigs, List<CharacterId> selectedCharacters, bool win)
        {
            const string increasedStatText = "+1 XP";
            
            foreach (var character in selectedCharacters)
            {
                var config = characterConfigs.GetConfig(character);
                var newCharacterView = new CharacterViewModel(config, _uiModelManager, showStatIncrease:win,
                    statIncreaseInfo: increasedStatText);
                SelectedCharacterViewModels.Add(newCharacterView);
            }
        }
        
        void ICombatResultWindowViewModel.HandleExitCombatButton()
        {
            _levelManager.LoadScene(GameState.CharacterSelection);
        }

        private string GetResultHeader(bool win)
        {
            return win ? "You WIN!" : "You LOSE!";
        }
    }
}
