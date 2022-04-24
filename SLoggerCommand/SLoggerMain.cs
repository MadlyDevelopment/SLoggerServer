using NLog;
using NLog.Web;
using SLoggerBusinessLogic.Tools;

namespace SLogger_Command;

public static class SLoggerMain
{
    
    private static readonly Logger _logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    
    public static void Main(string[] args)
    {
        _logger.Info("Starting up the SLogger server");
        StartUpSocketServer();
    }
    
    private static void StartUpSocketServer()
    {
        AsynchronousSocketListener.StartListening();
    }
}