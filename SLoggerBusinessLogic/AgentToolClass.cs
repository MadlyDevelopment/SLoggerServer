using System.Net;
using System.Net.Sockets;

namespace SLoggerBusinessLogic;

public class AgentToolClass
{
    private static Socket ConnectToServerSocket(IPAddress server, int port)
    {
        Socket socket = null;
        var ipe = new IPEndPoint(server, port);
        socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipe);
        if (!socket.Connected)
        {
            
        }
        return socket;
    }
}