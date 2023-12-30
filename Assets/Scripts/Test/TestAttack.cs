
using System.Collections;
using Combat;
using Commands.CombatCommands;
using GameConfig.Enum;
using GameServices;
using GameServices.ServiceLocator;
using Logger;
using Managers;
using ReactiveProperty;
using UnityEngine;

namespace Test
{
    public class TestAttack : MonoBehaviour, IDamageableEntity
    {
        public IReactiveProperty<float> CurrentHealth { get; }
        public float MaxHealth { get; }
        public float AttackPower => 1f;
        public bool IsEliminated { get; private set; }
        public Transform Transform => transform;
        
        public CombatState combatState;
        public TestAttack defenderTarget;

        public TestAttack(bool isEliminated, float maxHealth, IReactiveProperty<float> currentHealth)
        {
            IsEliminated = isEliminated;
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);

            if (combatState == CombatState.EnemyTurn)
            {
                GameServiceLocator.GetService<CombatServiceProvider>()
                    .GetManager<CombatManager>().EnemyTurnStarted += Attack;
            }
        }

        private void OnMouseUp()
        {
            Attack();
        }

        private void Attack()
        {
            GameServiceLocator.GetService<CombatServiceProvider>()
                .GetManager<CombatManager>().TryAttack(combatState, new MeleeAttackCommand(this,defenderTarget));
        }

        void IDamageableEntity.TakeDamage(float damageAmount)
        {
            DevLog.Log($"{gameObject.name} taken damage.");
        }
        
        void IDamageableEntity.Eliminated()
        {
            IsEliminated = true;
        }
    }
}
