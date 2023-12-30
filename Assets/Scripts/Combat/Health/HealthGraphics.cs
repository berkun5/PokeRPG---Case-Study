using UnityEngine;
using UnityEngine.UI;

namespace Combat.Health
{
    public class HealthGraphics : MonoBehaviour
    {
        [Header("HealthSlider")]
        [SerializeField] private Slider healthBarSlider;
        [SerializeField] private Image healthBarFillImage;
        
        private IHealthGraphicsViewModel _viewModel;
        private IDamageableEntity _observedEntity;
        
        private void OnDisable()
        {
            _observedEntity?.CurrentHealth.Unsubscribe(OnHealthChanged);
        }

        public void Init(IHealthGraphicsViewModel viewModel)
        {
            _viewModel = viewModel;
            healthBarSlider.maxValue = viewModel.MaxHealth;
            _observedEntity = viewModel.DamageableEntity;
            _observedEntity.CurrentHealth.Subscribe(OnHealthChanged);
        }
        
        private void OnHealthChanged(float currentHealth)
        {
            healthBarSlider.value = currentHealth;
            healthBarFillImage.color = _viewModel.GetHealthColor();
            
            if (_observedEntity.IsEliminated)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
