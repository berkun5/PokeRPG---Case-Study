using System;
using System.Collections.Generic;
using GameConfig.Enum;
using GameConfig.RemoteData;
using GameServices;
using GameServices.ServiceLocator;
using Managers.Base;

namespace Managers.UI
{
    public class CharacterSelectionModelManager : ManagerBase
    {
        public event Action<bool> MaxPartySizeReached;
        public event Action<int> PartySizeChanged;
        private Dictionary<CharacterId, bool> _selectedCharacters;
        private int _selectedCharacterCount;
        
        public override void Init()
        {
            _selectedCharacters = new Dictionary<CharacterId, bool>();

            var playerCharacters = GameServiceLocator.GetService<PersistentServiceProvider>()
                .GetManager<SettingsManager>().LocalData.CharacterConfigs.GetAllPlayerCharacters();
            
            foreach (var character in playerCharacters)
            {
                _selectedCharacters.Add(character.CharacterId, false);
            }
        }

        public override void LateStart()
        {
            
        }

        public bool TrySelectCharacter(CharacterId id)
        {
            var isSelected = _selectedCharacters[id];
    
            if (isSelected)
            {
                //deselecting character
                _selectedCharacters[id] = false;
                if (_selectedCharacterCount > 0)
                {
                    _selectedCharacterCount--;
                }
            }
            else if (_selectedCharacterCount < GameData.MaxCombatPartySize)
            {
                //selecting character
                _selectedCharacters[id] = true;
                _selectedCharacterCount++;
            }
    
            MaxPartySizeReached?.Invoke(_selectedCharacterCount >= GameData.MaxCombatPartySize);
            PartySizeChanged?.Invoke(_selectedCharacterCount);
            return _selectedCharacters[id];
        }
    }
}