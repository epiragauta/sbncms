using Serilog.Events;

namespace Sbn.Cms.Core.Logging.Viewer
{
    public interface ILogFilter
    {
        bool TakeLogEvent(LogEvent e);
    }
}
