using System.Collections.Generic;
using Commands.Base;
using Commands.Enum;
using Logger;
using Managers.Base;
using Managers.Interface;
using UnityEngine;

namespace Managers.CommandManagers
{
    public class CombatCommandManager : ManagerBase, IManagerUpdate
    {
        private readonly List<GameCommandType> _allowedCommandTypes = new()
        {
            GameCommandType.Combat,
        };
        
        private readonly Queue<Command> _commandsQueue = new();
        private Command _currentCommand;
        
        public override void Init()
        {
            
        }

        public override void LateStart()
        {
            
        }

        public bool QueueCommand(Command newCommand)
        {
            var newCommandType = newCommand.CommandType();
            if (!_allowedCommandTypes.Contains(newCommandType))
            {
                DevLog.LogWarning($"NotAllowed: CombatCommandManager will not queue command type of {newCommandType}.");
                return false;
            }
            _commandsQueue.Enqueue(newCommand);
            return true;
        }
        
         void IManagerUpdate.UpdateManager()
        {
            //pick command
            if (_commandsQueue.Count > 0 && _currentCommand == null)
            {
                _currentCommand = _commandsQueue.Dequeue();
            }

            if (_currentCommand == null)
            {
                return;
            }
            
            //execute command
            _currentCommand.Execute();
            TryCompleteCurrentCommand();
        }
        
        private void TryCompleteCurrentCommand()
        {
            if (_currentCommand != null && _currentCommand.IsCompleted())
            {
                _currentCommand = null;
            }
        }
    }
}
