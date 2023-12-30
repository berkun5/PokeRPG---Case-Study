using Commands.Base;
using Commands.Enum;

namespace Commands.CombatCommands
{
    //example attack command for scaling, it can ben paired with enum and populated
    public class RangedAttackCommand : Command
    {
        private const GameCommandType Type = GameCommandType.Combat;
        private bool _isCompleted;
        private bool _dataUpdated;
        
        public RangedAttackCommand()
        {
            
        }
        
        public override GameCommandType CommandType()
        {
            return Type;
        }

        public override void Execute()
        {
            
        }

        public override bool DataUpdated()
        {
            return _dataUpdated;
        }

        public override bool IsCompleted()
        {
            return _isCompleted;
        }
    }
}
