using GameConfig.LocalData;
using GameConfig.RemoteData;
using Logger;

namespace DataManagement
{
    public class RemoteDataProvider 
    {
        public GameData GameData { get; }
        public CharacterData CharacterData { get; }
        
        public RemoteDataProvider(CharacterConfigs characterConfigs)
        {
            GameData = new GameData();
            CharacterData = new CharacterData(characterConfigs);
            LoadAllData();
        }
        
        private void LoadAllData()
        {
            //async operation for remote server, but currently it's just player prefs
            DevLog.Log("Loading all data.");
            GameData.Load();
            CharacterData.Load();
        }
        
        public void SaveAllData()
        {
            DevLog.Log("Saving all data.");
            GameData.Save();
            CharacterData.Save();
        }
    }
}
