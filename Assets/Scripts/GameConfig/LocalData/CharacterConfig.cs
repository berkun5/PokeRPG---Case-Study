using Characters.Base;
using GameConfig.Enum;
using UnityEngine;

namespace GameConfig.LocalData
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/Character")]
    
    public class CharacterConfig : ScriptableObject
    {
        [SerializeField] private CharacterEntityBase characterPrefab;
        [Space(10)]
        [SerializeField] private CharacterId characterId;
        [SerializeField] private int baseExperience;
        [SerializeField] private int baseLevel = 1;
        [SerializeField] private float baseHealth;
        [SerializeField] private float baseAttackPower;
        [SerializeField] private Sprite visual;
        
        public CharacterEntityBase CharacterPrefab => characterPrefab;
        public CharacterId CharacterId => characterId;
        public int BaseExperience => baseExperience;
        public int BaseLevel => baseLevel;
        public float BaseHealth => baseHealth;
        public float BaseAttackPower => baseAttackPower;
        public Sprite Visual => visual;
        
    }
}
