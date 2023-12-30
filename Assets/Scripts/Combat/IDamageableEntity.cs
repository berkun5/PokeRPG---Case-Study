using ReactiveProperty;
using UnityEngine;

namespace Combat
{
    //any entity that can be damaged, can be lootBox, door, enemy, playerCharacter
    public interface IDamageableEntity
    {
        IReactiveProperty<float> CurrentHealth { get; }
        float MaxHealth { get; }
        float AttackPower { get; }
        bool IsEliminated { get; }
        Transform Transform { get; }
        void TakeDamage(float damageAmount);
        void Eliminated();
    }
}
