using Combat;
using Commands.Base;
using Commands.Enum;
using UnityEngine;

namespace Commands.CombatCommands
{
    public class MeleeAttackCommand : Command
    {
        private const GameCommandType Type = GameCommandType.Combat;
        private readonly IDamageableEntity _attacker;
        private readonly IDamageableEntity _defender;
        private readonly float _moveSpeed;
        private readonly Vector3 _attackerStartPosition;
        private bool _isCompleted;
        private bool _attackCompleted;
        private bool _isMovingToTarget = true;
        
        public MeleeAttackCommand(IDamageableEntity attacker, IDamageableEntity defender, float moveSpeed = 10f)
        {
            _attacker = attacker;
            _defender = defender;
            _moveSpeed = moveSpeed;
            _attackerStartPosition = attacker.Transform.position;
        }
        
        public override GameCommandType CommandType()
        {
            return Type;
        }

        public override void Execute()
        {
            if (_attacker.IsEliminated)
            {
                _attackCompleted = true;
                _isCompleted = true;
            }
            
            var targetPosition = _isMovingToTarget ? _defender.Transform.position : _attackerStartPosition;
            var timeStep = _moveSpeed * Time.deltaTime;
            _attacker.Transform.position = Vector3.Lerp(_attacker.Transform.position, targetPosition, timeStep);
            
            if (!_attackCompleted && (_attacker.Transform.position - targetPosition).magnitude < 0.01f)
            {
                // Switch direction
                _attacker.Transform.position = targetPosition;
                _defender.TakeDamage(_attacker.AttackPower);
                _isMovingToTarget = !_isMovingToTarget;
                _attackCompleted = true;
            }
            else if ((_attacker.Transform.position - _attackerStartPosition).magnitude < 0.01f)
            {
                _isCompleted = true;
            }
        }

        public override bool DataUpdated()
        {
            return _attackCompleted;
        }

        public override bool IsCompleted()
        {
            return _isCompleted;
        }
    }
}
