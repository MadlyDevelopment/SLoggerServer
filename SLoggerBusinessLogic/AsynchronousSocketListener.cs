using NLog;
using NLog.Web;

namespace SLoggerBusinessLogic;

public class AsynchronousSocketListener
{
    private static Logger _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    
    
}