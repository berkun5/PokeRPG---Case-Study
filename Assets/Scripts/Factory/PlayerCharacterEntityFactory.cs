using Characters.Base;
using Factory.Interface;
using GameConfig.LocalData;
using UnityEngine;

namespace Factory
{
    public class PlayerCharacterEntityFactory : ICharacterEntityFactory
    {
        CharacterEntityBase ICharacterEntityFactory.CreateCharacterEntity(CharacterConfig config, Transform parent, Vector3 position)
        {
            var player = Object.Instantiate(config.CharacterPrefab, parent);
            player.transform.position = position;
            player.Init(config);
            return player;
        }
    }
}
