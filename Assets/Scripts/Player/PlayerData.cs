using RuntimeBindings;

namespace Player
{
    public class PlayerData
    {
        private UpdatableField<int> _level = new();
        public UpdatableField<int> Level => _level;
    }
}