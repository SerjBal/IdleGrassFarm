using Serjbal.Core;

namespace Serjbal
{
    public interface IGame : IInitializable
    {
        void GameStart();
        void GameWin();
        void GameLoose();
        void GameStop();
    }
}
