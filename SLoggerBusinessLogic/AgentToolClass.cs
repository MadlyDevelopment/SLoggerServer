using System.Net;
using System.Net.Sockets;
using NLog;
using NLog.Web;

namespace SLoggerBusinessLogic;

public class AgentToolClass
{
    
    private static Logger _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    
}