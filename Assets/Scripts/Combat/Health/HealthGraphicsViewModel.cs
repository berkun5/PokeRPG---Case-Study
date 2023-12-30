using UnityEngine;

namespace Combat.Health
{
    public class HealthGraphicsViewModel : IHealthGraphicsViewModel
    {
        public IDamageableEntity DamageableEntity { get; }
        float IHealthGraphicsViewModel.MaxHealth => _maxHealth;
        private readonly float _maxHealth;
         public HealthGraphicsViewModel(IDamageableEntity observedEntity)
        {
            DamageableEntity = observedEntity;
            _maxHealth = observedEntity.MaxHealth;
        }

        Color IHealthGraphicsViewModel.GetHealthColor()
        {
            var currHp = DamageableEntity.CurrentHealth.Value;
            var normalizedHealth = Mathf.Clamp01(currHp / _maxHealth);
            var endColor = Color.Lerp(Color.red, Color.green, normalizedHealth);

            return endColor;
        }
         
    }
}
