using System.Collections.Generic;
using UnityEngine;

namespace DataManagement
{
    public abstract class RemoteDataBase
    {
        protected readonly Dictionary<string, object> Changes = new();
        protected abstract List<string> SaveFieldsKeys { get; }

        public void Save()
        {
            SetSave(GetChanges());
        }
        
        public void Load()
        {
            GetLoad(SaveFieldsKeys);
        }
        
        private void SetSave(IDictionary<string, object> updates)
        {
            foreach (var update in updates)
            {
                PlayerPrefs.SetString(update.Key, update.Value.ToString());
            }
            
            PlayerPrefs.Save();
        }
        
        private void GetLoad(List<string> keys)
        {
            var result = new Dictionary<string, object>(keys.Count);
            foreach (var key in keys)
            {
                result.Add(key, PlayerPrefs.GetString(key));
            }

            DataLoaded(result);
        }

        private IDictionary<string, object> GetChanges()
        {
            Changes.Clear();
            PrepareChangesToSave();
            
            return Changes;
        }
        
        protected abstract void PrepareChangesToSave();
        
        protected abstract void DataLoaded(IDictionary<string, object> data);
    }
}
