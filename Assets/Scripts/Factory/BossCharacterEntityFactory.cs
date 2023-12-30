using Characters.Base;
using Factory.Interface;
using GameConfig.LocalData;
using UnityEngine;

namespace Factory
{
    public class BossCharacterEntityFactory : ICharacterEntityFactory
    {
        CharacterEntityBase ICharacterEntityFactory.CreateCharacterEntity(CharacterConfig config, Transform parent, Vector3 position)
        {
            var boss = Object.Instantiate(config.CharacterPrefab, parent);
            boss.transform.position = position;
            boss.Init(config);
            return boss;
        }
    }
}
