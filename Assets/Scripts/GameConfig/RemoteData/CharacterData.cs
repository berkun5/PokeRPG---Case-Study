using System.Collections.Generic;
using System.Linq;
using DataManagement;
using GameConfig.Enum;
using GameConfig.LocalData;
using Logger;
using UnityEngine;

namespace GameConfig.RemoteData
{
    public class CharacterData : RemoteDataBase
    {
        //constants should be fetched from server to dynamically change without taking a new build
        private const int ExperiencePerVictory = 1;
        private const int RequiredExperiencePerLevel = 5;
        private const float CharacterAttributePercentagePerLevel = 10;
        
        protected override List<string> SaveFieldsKeys => _saveFieldsKeys;
        private readonly List<string> _saveFieldsKeys;
        private readonly Dictionary<CharacterId, int> _characterExperienceDict  = new();
        private readonly Dictionary<CharacterId, bool> _selectedPlayerCharactersDict  = new();
        private readonly Dictionary<CharacterId, bool> _selectedBossCharactersDict  = new();
        private readonly Dictionary<CharacterId, float> _activeCharactersHealthDict  = new();
        private readonly CharacterConfigs _localCharacterConfig;
        
        public CharacterData(CharacterConfigs characterConfigs)
        {
            _saveFieldsKeys = new List<string>();
            _localCharacterConfig = characterConfigs;
            
            var playerCharacters = _localCharacterConfig.GetAllPlayerCharacters();
            var allCharacters = _localCharacterConfig.GetAllCharacters();
            
            foreach (var playerCharacter in playerCharacters)
            {
                _saveFieldsKeys.Add(GetCharacterExperienceDataKey(playerCharacter.CharacterId));
            }
            
            foreach (var anyCharacter in allCharacters)
            {
                _saveFieldsKeys.Add(GetSelectedCombatCharacterDataKey(anyCharacter.CharacterId));
            }
            
            foreach (var combatCharacter in _localCharacterConfig.GetAllCharacters())
            {
                _saveFieldsKeys.Add(GetActiveCombatCharacterHealthDataKey(combatCharacter.CharacterId));
            }
        }
        
        protected override void PrepareChangesToSave()
        {
            foreach (var characterExp in _characterExperienceDict)
            {
                Changes.Add(GetCharacterExperienceDataKey(characterExp.Key), characterExp.Value);
            }
            
            foreach (var selectedCharacter in _selectedPlayerCharactersDict)
            {
                Changes.Add(GetSelectedCombatCharacterDataKey(selectedCharacter.Key), selectedCharacter.Value);
            }
            
            foreach (var selectedBoss in _selectedBossCharactersDict)
            {
                Changes.Add(GetSelectedCombatCharacterDataKey(selectedBoss.Key), selectedBoss.Value);
            }
            
            foreach (var activeCombatCharacter in _activeCharactersHealthDict)
            {
                Changes.Add(GetActiveCombatCharacterHealthDataKey(activeCombatCharacter.Key), activeCombatCharacter.Value);
            }
        }

        protected override void DataLoaded(IDictionary<string, object> data)
        {
            var playerCharacters = _localCharacterConfig.GetAllPlayerCharacters();
            var bossCharacters = _localCharacterConfig.GetAllBossCharacters();
            
            foreach (var playerChar in playerCharacters)
            {
                //Exp Data
                var characterExpKey = GetCharacterExperienceDataKey(playerChar.CharacterId);
                var characterExp = data.GetInt(characterExpKey); 
                _characterExperienceDict.TryAdd(playerChar.CharacterId, characterExp);
                
                //player character Selection Data
                var characterSelectionKey = GetSelectedCombatCharacterDataKey(playerChar.CharacterId);
                var characterSelected = data.GetBool(characterSelectionKey, false); 
                
                _selectedPlayerCharactersDict.TryAdd(playerChar.CharacterId, characterSelected);
            }
            
            
            foreach (var bossChar in bossCharacters)
            {
                //boss Selection Data
                var bossSelectionKey = GetSelectedCombatCharacterDataKey(bossChar.CharacterId);
                var bossSelected = data.GetBool(bossSelectionKey, false);
                _selectedBossCharactersDict.TryAdd(bossChar.CharacterId, bossSelected);
            }
            
            
            foreach (var combatCharacter in _localCharacterConfig.GetAllCharacters())
            {
                //Health Data
                var combatCharacterKey = GetActiveCombatCharacterHealthDataKey(combatCharacter.CharacterId);
                var defaultHealth = GetCharacterTotalHealth(combatCharacter.CharacterId);
                var loadedCharacterHealth = data.GetFloat(combatCharacterKey, defaultHealth); 
                
                _activeCharactersHealthDict.TryAdd(combatCharacter.CharacterId, loadedCharacterHealth);
            }
        }
        
        public List<CharacterId> GetSelectedCharacters()
        {
            var selectedCharacters = new List<CharacterId>();
            foreach (var character in _selectedPlayerCharactersDict)
            {
                if (character.Value)
                {
                    selectedCharacters.Add(character.Key);
                }
            }
            
            return selectedCharacters;
        }
        
        public void ClearSelectedCharacters()
        {
            var selectedCharactersList = _selectedPlayerCharactersDict.Keys.ToList();
            foreach (var key in selectedCharactersList)
            {
                _selectedPlayerCharactersDict[key] = false;
            }
        }
        
        public void SetSelectedCharacter(CharacterId id) => _selectedPlayerCharactersDict[id] = true;
        
        public void SetDeSelectedCharacter(CharacterId id) => _selectedPlayerCharactersDict[id] = false;
        
        public List<CharacterId> GetSelectedBossCharacters()
        {
            var selectedBosses = new List<CharacterId>();
            foreach (var character in _selectedBossCharactersDict)
            {
                if (character.Value)
                {
                    selectedBosses.Add(character.Key);
                }
            }
            
            return selectedBosses;
        }
        
        public void ClearSelectedBosses()
        {
            var selectedBossCharacterList = _selectedBossCharactersDict.Keys.ToList();
            foreach (var key in selectedBossCharacterList)
            {
                _selectedBossCharactersDict[key] = false;
            }
        }
        
        public void SetSelectedBossCharacter(CharacterId id) => _selectedBossCharactersDict[id] = true;
        
        public float GetCombatCharacterRemainingHealth(CharacterId id)
        {
            return _activeCharactersHealthDict[id];
        }
        
        //for heals and such
        public float AddCombatCharacterHealth(CharacterId id, float addHealthAmount)
        { 
            _activeCharactersHealthDict[id] += addHealthAmount;
            return _activeCharactersHealthDict[id];
        }
        
        //for damage taken
        public float ReduceCombatCharacterHealth(CharacterId id, float reduceHealthAmount)
        {
            _activeCharactersHealthDict[id] -= reduceHealthAmount;
            return _activeCharactersHealthDict[id];
        }
        
        public void ClearAllActiveCombatCharacterHealth()
        {
            var combatCharacters = _activeCharactersHealthDict.Keys.ToList();
            foreach (var key in combatCharacters)
            {
                _activeCharactersHealthDict[key] = GetCharacterTotalHealth(key);
            }
        }
        
        public void IncreaseCharacterExperience(CharacterId id)
        {
            var localConfig = _localCharacterConfig.GetConfig(id);
            var baseExperience = localConfig.BaseExperience;

            if (_characterExperienceDict.ContainsKey(id))
            {
                _characterExperienceDict[id] += ExperiencePerVictory;
            }
            else
            {
                _characterExperienceDict.Add(id, baseExperience + ExperiencePerVictory);
            }
        }
        
        public int GetCharacterExperience(CharacterId id)
        {
            var baseExperience = _localCharacterConfig.GetConfig(id).BaseExperience;
            return _characterExperienceDict.GetValueOrDefault(id, baseExperience);
        }
        
        public int GetCharacterLevel(CharacterId id)
        {
            var baseLevel = _localCharacterConfig.GetConfig(id).BaseLevel;
            var levelByExperience = GetCharacterExperience(id) / RequiredExperiencePerLevel;
            return baseLevel + levelByExperience;
        }

        public float GetCharacterAttackPower(CharacterId id)
        {
            //var tenP = baseAttack * 0.1f;
            var characterLevel = GetCharacterLevel(id);
            var baseAttack = _localCharacterConfig.GetConfig(id).BaseAttackPower;
            var increasePerLevel = baseAttack * (CharacterAttributePercentagePerLevel / 100);
            return baseAttack + (characterLevel * increasePerLevel);
        }
        
        public float GetCharacterTotalHealth(CharacterId id)
        {
            var characterLevel = GetCharacterLevel(id);
            var baseHealth = _localCharacterConfig.GetConfig(id).BaseHealth;
            var increasePerLevel = baseHealth * (CharacterAttributePercentagePerLevel / 100);
            return baseHealth + (characterLevel * increasePerLevel);
        }
        
        private string GetCharacterExperienceDataKey(CharacterId characterId)
        {
            return  $"{DataKeys.CharacterExperience}{(int)characterId}";
        }        
        
        private string GetSelectedCombatCharacterDataKey(CharacterId characterId)
        {
            return  $"{DataKeys.SelectedCombatCharacter}{(int)characterId}";
        }
        
        private string GetActiveCombatCharacterHealthDataKey(CharacterId characterId)
        {
            return  $"{DataKeys.ActiveCombatCharacterHealth}{(int)characterId}";
        }
    }
}
