using System;
using System.Collections;
using Combat;
using Commands.Base;
using Factory.Interface;
using Factory.Spawners;
using GameConfig.Enum;
using GameConfig.LocalData;
using GameConfig.RemoteData;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers.Base;
using Managers.CommandManagers;
using Managers.UI;
using ObservableList;
using UI.CombatResult;
using UI.FloatingText;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class CombatManager : ManagerBase
    {
        public event Action EnemyTurnStarted;

        [SerializeField] private CharacterEntitySpawner characterSpawner;
        
        private readonly ObservableList<IDamageableEntity> _bossCharacters = new();
        private readonly ObservableList<IDamageableEntity> _playerCharacters = new();
        
        private ICharacterEntityFactory _playerFactory;
        private ICharacterEntityFactory _bossFactory;
        
        private CombatCommandManager _combatCommandManager;
        private SettingsManager _settingsManager;
        private UIModelManager _uiModelManager;
        private Command _previousAttackCommand;
        
        private void OnDisable()
        {
            _bossCharacters.ItemRemoved-= TriggerPlayerWin;
            _playerCharacters.ItemRemoved-= TriggerPlayerLose;
        }

        public override void Init()
        {
            _bossCharacters.ItemRemoved += TriggerPlayerWin;
            _playerCharacters.ItemRemoved+= TriggerPlayerLose;
        }

        public override void LateStart()
        {

        }

        public void StartCombat()
        {
            _combatCommandManager = GameServiceLocator.GetService<CommandServiceProvider>()
                .GetManager<CombatCommandManager>();
            
            _settingsManager = GameServiceLocator.GetService<PersistentServiceProvider>()
                .GetManager<SettingsManager>();

            _uiModelManager = GameServiceLocator.GetService<UIModelServiceProvider>().GetManager<UIModelManager>();
            characterSpawner.TryInitialize(SpawnerBase.ExecutionType.Manual);
            
            SpawnAllCombatCharacters();

            if (_settingsManager.RemoteData.GameData.GetCombatState() == CombatState.EnemyTurn)
            {
                EnemyTurnStarted?.Invoke();
            }
        }
        
        private void TriggerPlayerWin(IDamageableEntity removedBossEntity, int remainingBossCount)
        {
            if (remainingBossCount > 0)
            {
                return;
            }
            
            var gameData = _settingsManager.RemoteData.GameData;
            gameData.IncreaseTotalPlayCount();
            gameData.SetCombatState(CombatState.Lose);
            
            var characterData = _settingsManager.RemoteData.CharacterData;
            var selectedChars = characterData.GetSelectedCharacters();
            
            foreach (var selectedCharacter in selectedChars)
            {
                characterData.IncreaseCharacterExperience(selectedCharacter);
            }
            
            _uiModelManager.Show<CombatResultWindow>(window => 
                window.Init(new CombatResultWindowViewModel(_uiModelManager,_settingsManager, selectedChars, true)));
        }
        
        private void TriggerPlayerLose(IDamageableEntity removedPlayerEntity, int remainingPlayerCount)
        {
            if (remainingPlayerCount > 0)
            {
                return;
            }
            
            var gameData = _settingsManager.RemoteData.GameData;
            gameData.IncreaseTotalPlayCount();
            gameData.SetCombatState(CombatState.Lose);
            
            var selectedChars = _settingsManager.RemoteData.CharacterData.GetSelectedCharacters();
            _uiModelManager.Show<CombatResultWindow>(window => 
                window.Init(new CombatResultWindowViewModel(_uiModelManager,_settingsManager, selectedChars, false)));
        }

        private void SpawnAllCombatCharacters()
        {
            var characterData = _settingsManager.RemoteData.CharacterData;
            var localCharacterConfigs = _settingsManager.LocalData.CharacterConfigs;

            SpawnPlayerCharacters(characterData, localCharacterConfigs);
            SpawnBossCharacters(characterData, localCharacterConfigs);
        }

        private void SpawnPlayerCharacters(CharacterData characterData, CharacterConfigs localCharacterConfigs)
        {
            //spawn characters
            var selectedCharacters = characterData.GetSelectedCharacters();
            foreach (var selected in selectedCharacters)
            {
                var previousSavedHp= characterData.GetCombatCharacterRemainingHealth(selected);
                if (previousSavedHp <= 0)
                {
                    //skip character spawn if already eliminated
                    continue;
                }
                
                var selectedCharacterConfig = localCharacterConfigs.GetConfig(selected);
                var spawnedCharacter = characterSpawner.SpawnPlayerCharacter(selectedCharacterConfig);
                _playerCharacters.Add(spawnedCharacter);
            }
        }

        private void SpawnBossCharacters(CharacterData characterData, CharacterConfigs localCharacterConfigs)
        {
            var loadedBosses = characterData.GetSelectedBossCharacters();
            
            //new boss
            if (loadedBosses.Count <= 0)
            {
                //the system supports multiple boss creation if needed
                var bossCharacters = localCharacterConfigs.GetAllBossCharacters();
                var bossConfig = bossCharacters[Random.Range(0, bossCharacters.Count)];
                var spawnedBoss = characterSpawner.SpawnBoss(bossConfig);
                
                characterData.SetSelectedBossCharacter(bossConfig.CharacterId);
                _bossCharacters.Add(spawnedBoss);
                return;
            } 
            
            //loaded bosses
            foreach (var boss in loadedBosses)
            {
                var bossConfig = localCharacterConfigs.GetConfig(boss);
                var spawnedBoss = characterSpawner.SpawnBoss(bossConfig);
                _bossCharacters.Add(spawnedBoss);
            }
        }
        
        public IDamageableEntity TryGetRandomBossAsTarget(out bool noTarget)
        { 
            noTarget = _bossCharacters.Count <= 0;
            var rand = Random.Range(0,_bossCharacters.Count);
            return noTarget? null : _bossCharacters[rand];
        }
        
        public IDamageableEntity TryGetRandomPlayerCharacterAsTarget(out bool noTarget)
        {
            noTarget = _bossCharacters.Count <= 0;
            var rand = Random.Range(0,_playerCharacters.Count);
            return noTarget ? null : _playerCharacters[rand];
        }
        
        public void TryAttack(CombatState attackerTurnType, Command attackCommand)
        {
            var currentCombatState =  _settingsManager.RemoteData.GameData.GetCombatState();

            if (PlayerShouldWaitForEnemyTurn(currentCombatState))
            {
                return;
            }
            
            if (attackerTurnType != currentCombatState)
            {
                //wrong turn type
                return;
            }
            
            //attack request queued up
            var queueSuccess = _combatCommandManager.QueueCommand(attackCommand);
            if (!queueSuccess)
            {
                return;
            }
            
            _previousAttackCommand = attackCommand;
            TrySwitchCombatTurn(currentCombatState);
        }

        public void TryTakeDamage(IDamageableEntity entity, CharacterId id, float receivedDamage)
        {
            var remainingHealth = _settingsManager.RemoteData.CharacterData.ReduceCombatCharacterHealth(id, receivedDamage);
            entity.CurrentHealth.Value = remainingHealth;
            
            _uiModelManager.Show<FloatingTextPanel>(panel =>
                panel.Init(new FloatingTextPanelViewModel(receivedDamage.ToString("F1"), entity.Transform.position)));
            DevLog.Log($"{id} has taken {receivedDamage} damage. Remaining HP: {remainingHealth}");
            
            if (remainingHealth > 0)
            {
                return;
            }
            
            if (_playerCharacters.Contains(entity))
            {
                _playerCharacters.Remove(entity);
            }
            
            if (_bossCharacters.Contains(entity))
            {
                _bossCharacters.Remove(entity);
            }
            
            entity.Eliminated();
            DevLog.Log($"{id} has been Eliminated!!");
        }
        
        private void TrySwitchCombatTurn(CombatState currentCombatState)
        {
            //"--Try" because this can be elaborated further depending on the gameDesign,
            //remaining movement, buff, etc.. right now we just have single attack per turn
            var gameData = _settingsManager.RemoteData.GameData;
            switch (currentCombatState)
            {
                case CombatState.EnemyTurn:
                    StartCoroutine(EndBossTurn());
                    //gameData.SetCombatState(CombatState.PlayerTurn);
                    break;
                case CombatState.PlayerTurn:
                    gameData.SetCombatState(CombatState.EnemyTurn);
                    EnemyTurnStarted?.Invoke();
                    break;
                
                case CombatState.Win:
                case CombatState.Lose:
                default:
                    break;
            }
        }

        private bool PlayerShouldWaitForEnemyTurn(CombatState currentCombatState)
        {
            var isPlayerTurn = currentCombatState == CombatState.PlayerTurn;
            var previousCommandInProgress = _previousAttackCommand != null && !_previousAttackCommand.IsCompleted();

            return isPlayerTurn && previousCommandInProgress;
        }

        private IEnumerator EndBossTurn()
        {
            var frame = new WaitForEndOfFrame();
            while (_previousAttackCommand != null && !_previousAttackCommand.DataUpdated())
            {
                yield return frame;
            }
            
            _settingsManager.RemoteData.GameData.SetCombatState(CombatState.PlayerTurn);
        }
    }
}

