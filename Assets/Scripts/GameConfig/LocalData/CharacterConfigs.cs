using System.Collections.Generic;
using System.Linq;
using DataManagement;
using GameConfig.Enum;
using UnityEngine;

namespace GameConfig.LocalData
{
    [CreateAssetMenu(fileName = "CharacterConfigList", menuName = "ScriptableObjects/Lists/CharacterConfigList", order = 99)]
    public class CharacterConfigs : ScriptableObject
    {
        [Header("Configs")]
        [SerializeField] private Sprite lockedCharacterImage;
        [SerializeField] private List<CharacterConfig> characters;
        
        public Sprite GetLockedCharacterImage()
        {
            return lockedCharacterImage;
        }

        public CharacterConfig GetConfig(CharacterId characterId)
        {
            var character = characters.FirstOrDefault(config => config.CharacterId == characterId);
            if (character)
            {
                return character;
            }

            Debug.LogWarning($"Settings for character {characterId} can not found.");
            return null;
        }

        public List<CharacterConfig> GetAllCharacters()
        {
            return characters.ToList();
        }
        
        public List<CharacterConfig> GetAllPlayerCharacters()
        {
            var playerCharacters = new List<CharacterConfig>();
            foreach (var character in characters)
            {
                if (IsBossCharacter(character.CharacterId))
                {
                    continue;
                }
                
                playerCharacters.Add(character);
            }
            
            return playerCharacters;
        }
        
        public List<CharacterConfig> GetAllBossCharacters()
        {
            var bossCharacters = new List<CharacterConfig>();
            foreach (var character in characters)
            {
                if (!IsBossCharacter(character.CharacterId))
                {
                    continue;
                }
                
                bossCharacters.Add(character);
            }
            
            return bossCharacters;
        }

        public bool IsBossCharacter(CharacterId character)
        {
            return character.ToString().Contains(DataKeys.BossIdentifier);
        }
    }
}
