using Zenject;

namespace Player
{
    public interface IPlayerDataManager : IInitializable
    {
        PlayerData Data { get; }
        void IncrementLevel();
    }
}