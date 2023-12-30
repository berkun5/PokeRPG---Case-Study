using Combat;
using Combat.Health;
using GameConfig.LocalData;
using GameConfig.RemoteData;
using GameServices;
using GameServices.ServiceLocator;
using Managers;
using ReactiveProperty;
using UnityEngine;

namespace Characters.Base
{
    public abstract class CharacterEntityBase : MonoBehaviour, IDamageableEntity
    {
        public bool IsEliminated { get; private set; }
        public IReactiveProperty<float> CurrentHealth => _currentHealth;
        float IDamageableEntity.MaxHealth => _maxHealth;
        float IDamageableEntity.AttackPower => _attackPower;
        Transform IDamageableEntity.Transform => transform;
        
        protected bool IsInitialized;
        protected CombatManager CombatManager;
        protected CharacterConfig Config;
        
        [Header("Graphics")]
        [SerializeField] protected SpriteRenderer characterSpriteRenderer;
        [SerializeField] private HealthGraphics healthGraphics;

        private readonly ReactiveProperty<float> _currentHealth = new();

        private float _attackPower;
        private float _maxHealth;
        private SettingsManager _settingsManager;
        private CharacterData _characterData;

        public void Init(CharacterConfig config)
        {
            if (IsInitialized)
            {
                return;
            }
            
            Config = config;
            Initialize();
        }
        
        protected virtual void Initialize()
        {
            _settingsManager = GameServiceLocator.GetService<PersistentServiceProvider>().GetManager<SettingsManager>();
            _characterData = _settingsManager.RemoteData.CharacterData;
            CombatManager = GameServiceLocator.GetService<CombatServiceProvider>().GetManager<CombatManager>();
            
            _currentHealth.Value = _characterData.GetCombatCharacterRemainingHealth(Config.CharacterId);
            _attackPower = _characterData.GetCharacterAttackPower(Config.CharacterId);
            _maxHealth = _characterData.GetCharacterTotalHealth(Config.CharacterId);
            
            SetGraphics();
            IsInitialized = true;
            
        }

        protected virtual void SetGraphics()
        {
            characterSpriteRenderer.sprite = Config.Visual;
            healthGraphics.Init(new HealthGraphicsViewModel(this));
        }
        
        protected abstract void Attack();
        
        void IDamageableEntity.TakeDamage(float damageAmount)
        {
            CombatManager.TryTakeDamage(this, Config.CharacterId, damageAmount);
        }

        void IDamageableEntity.Eliminated()
        {
            IsEliminated = true;
            gameObject.SetActive(false);
        }
    }
}
