using RuntimeBindings;
using Shared.Types;
using Zenject;

namespace LOEvents
{
    public interface ILOEventsManager : IInitializable
    {
        IReadOnlyUpdatableField<LOEventBaseData[]> Data { get; }
        void OnEventOpened(LOEventBaseData eventData);
        void OnOpenedExpiredEvent(LOEventBaseData eventData);
    }
}