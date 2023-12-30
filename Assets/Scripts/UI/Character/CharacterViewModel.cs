using GameConfig.LocalData;
using Logger;
using Managers;
using Managers.UI;
using ReactiveProperty;
using UI.CharacterSelection.Popup;
using UI.Popup;
using UnityEngine;

namespace UI.Character
{
    public class CharacterViewModel : ICharacterViewModel
    {
        public Sprite CharacterIcon { get; }
        public bool ShowStatIncrease { get; }
        public string StatIncreaseInfo { get; }
        public IReactiveProperty<Color> CharacterImageColor => _characterImageColor;
        public IReactiveProperty<bool> CharacterSelected => _characterSelected;
        
        private readonly ReactiveProperty<Color> _characterImageColor = new();
        private readonly ReactiveProperty<bool> _characterSelected = new();
        
        private readonly UIModelManager _uiModelManager;
        private readonly CharacterSelectionModelManager _selectionModelManager;
        private readonly SettingsManager _settingsManager;
        
        private readonly CharacterConfig _config;
        private readonly Color _darkenColor = new(.6f,.6f,.6f,1);
        private readonly Color _lightenColor = new(1,1,1,1);
        private readonly bool _unlocked;
        private readonly bool _selectable;
        private bool _longPressDetected;
        
        public CharacterViewModel(CharacterConfig config, UIModelManager uiModelManager, 
            CharacterSelectionModelManager modelManager = null, SettingsManager settingsManager = null, Sprite lockedCharacterSprite = null,
            bool unlocked = true, bool selectable = false, bool showStatIncrease = false, string statIncreaseInfo = "")
        {
            _config = config;
            _uiModelManager = uiModelManager;
            _unlocked = unlocked;
            _selectable = selectable;
            ShowStatIncrease = showStatIncrease;
            StatIncreaseInfo = statIncreaseInfo;
            CharacterIcon = unlocked ? config.Visual : lockedCharacterSprite;

            if (!selectable)
            {
                return;
            }
            
            _settingsManager = settingsManager;
            _selectionModelManager = modelManager;
            _selectionModelManager.MaxPartySizeReached += DarkenCharacterView;
        }
        
        public void Dispose()
        {
            if (_selectable)
            {
                _selectionModelManager.MaxPartySizeReached -= DarkenCharacterView;
            }
        }
        
        void DarkenCharacterView(bool maxSizeReached)
        {
            if (!_unlocked || !_selectable)
            {
                return;
            }
            
            if (maxSizeReached && !CharacterSelected.Value)
            {
                if (_characterImageColor.Value != _darkenColor)
                {
                    _characterImageColor.Value = _darkenColor;
                }
            }
            else if (_characterImageColor.Value != _lightenColor)
            { 
                _characterImageColor.Value = _lightenColor;
            }
        }
        
        void ICharacterViewModel.HandleSelectionLogic()
        {
            if (!_unlocked || !_selectable)
            {
                return;
            }

            if (_longPressDetected)
            {
                return;
            }
            
            CharacterSelected.Value = _selectionModelManager.TrySelectCharacter(_config.CharacterId);
            if (CharacterSelected.Value)
            {
                _characterImageColor.Value = _lightenColor;
                _settingsManager.RemoteData.CharacterData.SetSelectedCharacter(_config.CharacterId);
            }
            else
            {
                _settingsManager.RemoteData.CharacterData.SetDeSelectedCharacter(_config.CharacterId);
            }
        }
        
        void ICharacterViewModel.HandleToggleStatsPopup(bool show,Vector2 viewRectPosition)
        {
            _longPressDetected = show;
            if (show)
            {
                TryShowStatsPopup(viewRectPosition);
            }
            else
            {
                TryHideStatsPopup();
            }
        }

        private void TryShowStatsPopup(Vector2 viewRectPos)
        {
            if (!_unlocked)
            {
                return;
            }

            TryHideStatsPopup();
            _uiModelManager.Show<CharacterStatsPopupWindow>(window => 
                window.Init(new CharacterStatsPopupWindowViewModel(_config.CharacterId, viewRectPos)));
        }

        private void TryHideStatsPopup() => _uiModelManager.Hide<CharacterStatsPopupWindow>();
    }
}
