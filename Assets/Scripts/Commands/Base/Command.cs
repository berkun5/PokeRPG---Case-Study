using Commands.Enum;

namespace Commands.Base
{
    public abstract class Command
    {
        public abstract GameCommandType CommandType();
        
        public abstract void Execute();
        
        //when it is safe to access and change remote data that has dependency on this command
        public abstract bool DataUpdated();
        
        //when it is fully completed and can be disposed
        public abstract bool IsCompleted();
    }
}