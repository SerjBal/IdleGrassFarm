namespace Serjbal.Core
{
    public interface IState : IExecutable
    {
        public bool Enter();
        public bool Exit();
    }
}