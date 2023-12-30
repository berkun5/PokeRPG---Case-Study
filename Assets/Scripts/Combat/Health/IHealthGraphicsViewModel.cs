using UnityEngine;

namespace Combat.Health
{
    public interface IHealthGraphicsViewModel
    {
        Color GetHealthColor();
        float MaxHealth { get; }
        IDamageableEntity DamageableEntity { get; }
    }
}
