using System.Collections.Generic;
using System.Text;
using GameConfig.Enum;
using GameConfig.LocalData;
using GameConfig.RemoteData;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers;
using Managers.UI;
using ReactiveProperty;
using UI.Character;

namespace UI.CharacterSelection
{
    public class CharacterSelectionWindowViewModel : ICharacterSelectionWindowViewModel
    {
        private readonly CharacterSelectionModelManager _modelManager;
        private readonly LevelManager _levelManager;
        private readonly SettingsManager _settingsManager;
        private readonly UIModelManager _uiModelManager;
        
        public string SetNewCharacterCountText => _setNewCharacterCountText;
        public IReactiveProperty<bool> ToggleStartCombatButton => _maxPartySizeReached;
        public IReactiveProperty<string> SetSelectionInformationText => _setSelectionInformationText;
        public List<ICharacterViewModel> AllCharacterViewModels { get; } = new();
        private readonly ReactiveProperty<bool> _maxPartySizeReached = new();
        private readonly ReactiveProperty<string> _setSelectionInformationText = new();
        private readonly string _setNewCharacterCountText;
        public CharacterSelectionWindowViewModel(CharacterConfigs characterConfigs, int unlockedCharacterCount)
        {
            _uiModelManager = GameServiceLocator.GetService<UIModelServiceProvider>()
                .GetManager<UIModelManager>();
            
            _modelManager = GameServiceLocator.GetService<UIModelServiceProvider>()
                .GetManager<CharacterSelectionModelManager>();
            
            var persistentServiceProvider = GameServiceLocator.GetService<PersistentServiceProvider>();
            _levelManager = persistentServiceProvider.GetManager<LevelManager>();
            _settingsManager = persistentServiceProvider.GetManager<SettingsManager>();
            
            _modelManager.MaxPartySizeReached += OnMaxPartySizeReached;
            _modelManager.PartySizeChanged += OnPartySizeChanged;

            _setNewCharacterCountText = GetCharacterCountText();
                
            OnPartySizeChanged(0);
            ClearPreviousCombatData();
            InitCharacterViewModels(characterConfigs, unlockedCharacterCount);
        }

        public void Dispose()
        {
            _modelManager.MaxPartySizeReached -= OnMaxPartySizeReached;
        }

        private void ClearPreviousCombatData()
        {
            //can be moved to model manager
            var characterData = _settingsManager.RemoteData.CharacterData;
            characterData.ClearSelectedCharacters();
            characterData.ClearSelectedBosses();
            characterData.ClearAllActiveCombatCharacterHealth();
        }

        private string GetCharacterCountText()
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("Unlock new character in ");
            sBuilder.Append(_settingsManager.RemoteData.GameData.GetRemainingCombatCountForNewCharacter());
            sBuilder.Append(" Combat!");
            return sBuilder.ToString();
        }
        
        private void OnMaxPartySizeReached(bool sizeReached)
        {
            _maxPartySizeReached.Value = sizeReached;
        }

        private void InitCharacterViewModels(CharacterConfigs characterConfigs, int unlockedCharacterCount)
        {
            var playerCharacters = characterConfigs.GetAllPlayerCharacters();
            var lockedCharacterSprite = characterConfigs.GetLockedCharacterImage();
            
            foreach (var config in playerCharacters)
            {
                var unlocked = unlockedCharacterCount > 0;
                unlockedCharacterCount--;
                
                var newCharacterView = new CharacterViewModel(config, _uiModelManager, _modelManager, 
                    _settingsManager, lockedCharacterSprite, unlocked, true);
                AllCharacterViewModels.Add(newCharacterView);
            }
        }
        
        private void OnPartySizeChanged(int partySize)
        {
            if (_maxPartySizeReached.Value)
            {
                _setSelectionInformationText.Value = "READY TO GO!";
            }
            else
            {
                var remaining = GameData.MaxCombatPartySize - partySize;
                _setSelectionInformationText.Value = $"SELECT {remaining} MORE CHARACTER!";
            }
        }
        
        void ICharacterSelectionWindowViewModel.HandleStartCombatLogic()
        {
            const GameState targetGameState = GameState.Combat;
            _settingsManager.RemoteData.GameData.SetCombatState(CombatState.PlayerTurn); //maybe move to game manager
            _levelManager.LoadScene(targetGameState);
        }
        
    }
}
