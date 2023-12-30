using System.Collections.Generic;
using Characters.Base;
using Factory.Interface;
using GameConfig.LocalData;
using Logger;
using UnityEngine;

namespace Factory.Spawners
{
    public class CharacterEntitySpawner : SpawnerBase
    {
        [SerializeField] private List<Transform> playerCharacterSpawnPositions = new();
        [SerializeField] private List<Transform> bossCharacterSpawnPositions = new(); 
        
        private int _playerPositionIndex = 0;
        private int _bossPositionIndex = 0;
        
        private ICharacterEntityFactory _playerCharacterFactory;
        private ICharacterEntityFactory _bossCharacterFactory;

        protected override void Init()
        {
            base.Init();
            _playerCharacterFactory = new PlayerCharacterEntityFactory();
            _bossCharacterFactory = new BossCharacterEntityFactory();
            //...auto spawn on init if required
        }
        
        public CharacterEntityBase SpawnBoss(CharacterConfig bossConfig)
        {
            if (bossCharacterSpawnPositions.Count == 0)
            {
                DevLog.LogError("no boss character spawn positions available");
                return null;
            }
            
            var bossSpawnPoint = bossCharacterSpawnPositions[_bossPositionIndex];
            _bossPositionIndex++;
            
            if (_bossPositionIndex >= bossCharacterSpawnPositions.Count)
            {
                _bossPositionIndex = 0;
            }
            
            return _bossCharacterFactory.CreateCharacterEntity(bossConfig, bossSpawnPoint ,bossSpawnPoint.position);
        }
        
        public CharacterEntityBase SpawnPlayerCharacter(CharacterConfig playerCharacterConfig)
        {
            if (playerCharacterSpawnPositions.Count == 0)
            {
                DevLog.LogError("no player character spawn positions available");
                return null;
            }
            
            //loop spawn positions for possible null ref when spawning too many character
            var playerSpawnPoint = playerCharacterSpawnPositions[_playerPositionIndex];
            _playerPositionIndex++;
            
            if (_playerPositionIndex >= playerCharacterSpawnPositions.Count)
            {
                _playerPositionIndex = 0;
            }
            
            return _playerCharacterFactory.CreateCharacterEntity(playerCharacterConfig, playerSpawnPoint, playerSpawnPoint.position);
        }
    }
}
