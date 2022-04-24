using NLog;
using NLog.Web;

namespace SLoggerBusinessLogic.Tools;

public class AgentToolClass
{
    
    private static Logger _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    
}