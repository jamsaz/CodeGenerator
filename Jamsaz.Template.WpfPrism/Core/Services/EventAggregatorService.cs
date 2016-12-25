using Prism.Events;

namespace $safeprojectname$.Services
{
    public class EventAggregatorService
    {
        private static IEventAggregator instance;
        public static IEventAggregator Instance => instance;

        public static void SetEventAggregator(IEventAggregator eventAggregator)
        {
            instance = eventAggregator;
        }
    }
}
