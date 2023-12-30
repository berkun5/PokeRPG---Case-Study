using Characters.Base;
using Commands.CombatCommands;
using GameConfig.Enum;

namespace Characters
{
    public class BossCharacterEntity : CharacterEntityBase
    {
        private const CombatState RequiredCombatState = CombatState.EnemyTurn;
        
        private void OnDisable()
        {
            CombatManager.EnemyTurnStarted -= Attack;
        }

        private void OnEnable()
        {
            if (!IsInitialized)
            {
                return;
            }
            
            CombatManager.EnemyTurnStarted += Attack;
        }

        protected override void Initialize()
        {
            base.Initialize();
            CombatManager.EnemyTurnStarted -= Attack;
            CombatManager.EnemyTurnStarted += Attack;
        }

        protected override void Attack()
        {
            if (IsEliminated)
            {
                return;
            }
            
            var playerCharacter= CombatManager.TryGetRandomPlayerCharacterAsTarget(out var noPlayerTarget);
           
            if (noPlayerTarget)
            {
                return;
            }

            var attackCommand = new MeleeAttackCommand(this, playerCharacter);
            CombatManager.TryAttack(RequiredCombatState, attackCommand);
        }
        
        protected override void SetGraphics()
        {
            base.SetGraphics();
            //maybe dynamic scale for boss
            //add boss specific particles and such
        }
    }
}
