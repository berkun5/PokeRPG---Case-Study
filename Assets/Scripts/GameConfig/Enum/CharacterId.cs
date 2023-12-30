using UnityEngine;

namespace GameConfig.Enum
{
    public enum CharacterId
    {
        // Unique values [0, 9];
        None = 0,
        Random = 1,
        //...
        
        // Player Characters [10,200]:
        [InspectorName("Characters/Player/Togepi")]
        Togepi = 10,
        [InspectorName("Characters/Player/Pikachu")]
        Pikachu = 11,
        [InspectorName("Characters/Player/Bulbasaur")]
        Bulbasaur = 12,
        [InspectorName("Characters/Player/Squirtle")]
        Squirtle = 13,
        [InspectorName("Characters/Player/Charmander")]
        Charmander = 14,
        [InspectorName("Characters/Player/Psyduck")]
        Psyduck = 15,
        [InspectorName("Characters/Player/Ninetales")]
        Ninetales = 16,
        [InspectorName("Characters/Player/Gyarados")]
        Gyarados = 17,
        [InspectorName("Characters/Player/Infernape")]
        Infernape = 18,
        [InspectorName("Characters/Player/Celebi")]
        Celebi = 19,
        //...
        
        // Boss Characters [200, 300]: Must start with "Boss" tag
        [InspectorName("Characters/Boss/Mewtwo")]
        BossMewtwo = 200,
        [InspectorName("Characters/Boss/Kyogre")]
        BossKyogre = 201,
        [InspectorName("Characters/Boss/Giratina")]
        BossGiratina = 202,
        //...
    }
}
