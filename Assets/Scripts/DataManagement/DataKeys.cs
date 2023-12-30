namespace DataManagement
{ 
    //keys for saving and loading data
    public static class DataKeys
    {
        // Game Data
        public const string ActiveGameState = "ActiveGameState_";
        public const string ActiveCombatState = "ActiveCombatState_";
        public const string TotalCombatCount = "TotalCombatCount_";
        
        //Player Characters Data
        public const string BossIdentifier = "Boss";
        public const string CharacterExperience = "CharacterExperience_";
        public const string SelectedCombatCharacter = "SelectedCombatCharacter_";
        public const string ActiveCombatCharacterHealth = "ActiveCombatCharacterHealth_";
    }
}