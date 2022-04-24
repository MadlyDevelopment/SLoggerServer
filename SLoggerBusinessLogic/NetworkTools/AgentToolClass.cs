using NLog;
using NLog.Web;

namespace SLoggerBusinessLogic.NetworkTools;

public class AgentToolClass
{
    
    private static Logger _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    
}