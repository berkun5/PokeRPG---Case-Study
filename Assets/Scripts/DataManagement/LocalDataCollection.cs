using GameConfig.LocalData;
using UnityEngine;

namespace DataManagement
{
    //root for all local data.
    [CreateAssetMenu(fileName = "LocalDataCollection", menuName = "ScriptableObjects/LocalDataCollection")]
    public class LocalDataCollection : ScriptableObject
    {
        public CharacterConfigs CharacterConfigs => characterConfigs;
        [SerializeField] private CharacterConfigs characterConfigs;
    }
}
