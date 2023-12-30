using Characters.Base;
using GameConfig.LocalData;
using UnityEngine;

namespace Factory.Interface
{
    public interface ICharacterEntityFactory
    {
        //populate for different type of entities,
        //eg.CreateCharacterEntity(CharacterConfig config, Vector3 spawnPosition, Quaternion spawnRotation, AttackType aT);
        CharacterEntityBase CreateCharacterEntity(CharacterConfig config, Transform parent, Vector3 spawnPosition);
    }
}
