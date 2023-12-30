using System.Collections.Generic;
using DataManagement;
using GameConfig.Enum;

namespace GameConfig.RemoteData
{
    public class GameData : RemoteDataBase
    {
        //constants normally should be fetched from server to dynamically change without taking a new build
        public const int MaxCombatPartySize = 3;
        private const int RequiredCombatPerNewCharacter = 5;
        
        private GameState _activeGameState = GameState.CharacterSelection;
        private CombatState _activeCombatState = CombatState.PlayerTurn;
        private int _totalCombatCount;
        
        protected override List<string> SaveFieldsKeys { get; } = new()
        {
            DataKeys.ActiveGameState,
            DataKeys.ActiveCombatState,
            DataKeys.TotalCombatCount
        };
        
        public void SetGameState(GameState newGameState) => _activeGameState = newGameState;
        public GameState GetGameState()
        {
            return _activeGameState;
        }

        public void SetCombatState(CombatState newCombatState) => _activeCombatState = newCombatState;
        
        public CombatState GetCombatState()
        {
            return _activeCombatState;
        }
        
        public void ResetTotalPlayCount() => _totalCombatCount = 0;
        
        public void IncreaseTotalPlayCount() => _totalCombatCount++;
        
        public int GetUnlockedCharacterCount()
        {
            var unlockedIterations = _totalCombatCount / RequiredCombatPerNewCharacter;
            return MaxCombatPartySize + unlockedIterations;
        }
        
        public int GetRemainingCombatCountForNewCharacter()
        {
            return RequiredCombatPerNewCharacter - (_totalCombatCount % RequiredCombatPerNewCharacter);
        }
        
        protected override void PrepareChangesToSave()
        {
            Changes.Add(DataKeys.ActiveGameState, (int)_activeGameState);
            Changes.Add(DataKeys.ActiveCombatState, (int)_activeCombatState);
            Changes.Add(DataKeys.TotalCombatCount, _totalCombatCount);
        }

        protected override void DataLoaded(IDictionary<string, object> data)
        {
            _activeGameState = (GameState)data.GetInt(DataKeys.ActiveGameState);
            _activeCombatState = (CombatState)data.GetInt(DataKeys.ActiveCombatState);
            _totalCombatCount = data.GetInt(DataKeys.TotalCombatCount,0); //add default value as 100 > for hacking
        }
    }
}
