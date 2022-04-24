using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;
using NLog.Web;

namespace SLoggerBusinessLogic.Tools;

public class AsynchronousSocketListener
{

    private static int _port = 11000;
    
    private static Logger _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    
    public static void StartListening()
    {
        var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        var ipAdress = ipHostInfo.AddressList[0];
        var localEndPoint = new IPEndPoint(ipAdress, _port);
        _logger.Info($"The SLogger Server listen on {ipAdress}:{_port}");
        var listener = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);
            while (true)
            {
                allDone.Reset();
                _logger.Info("Waiting for a connection...");  
                listener.BeginAccept(
                    data => AcceptCallback(data),  
                    listener );
                allDone.WaitOne(); 
            }
        }
        catch (Exception e)
        {
            _logger.Error($"The Server could not start lisiting on port {_port}", e);
        }
    }
    public static void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.  
        allDone.Set();
        // Get the socket that handles the client request.  
        var listener = (Socket) ar.AsyncState!;  
        var handler = listener?.EndAccept(ar);
        var state = new StateObject
        {
            WorkSocket = handler
        };
        handler?.BeginReceive( state.Buffer, 0, StateObject.BufferSize, 0,  
            new AsyncCallback(data => ReadCallback(data)), state);  
    }
    
    public static void ReadCallback(IAsyncResult ar)
    {
        string content;  
  
        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        var state = (StateObject) ar.AsyncState!;  
        var handler = state.WorkSocket;  
  
        // Read data from the client socket.
        if (handler != null)
        {
            var bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0) 
            {
                state.Sb.Append(Encoding.UTF8.GetString(  
                    state.Buffer, 0, bytesRead));
                content = state.Sb.ToString();
                _logger.Debug($"Read {content.Length} bytes from socket.");
                _logger.Debug($"The following data was retrieved {content}");
                Send(handler, content);
            }  
        }
    }
    
    private static void Send(Socket handler, string data)
    {
        // Convert the string data to byte data using UTF8 encoding.  
        var byteData = Encoding.UTF8.GetBytes(data);  
  
        // Begin sending the data to the remote device.  
        handler.BeginSend(byteData, 0, byteData.Length, 0,  
            new AsyncCallback(SendCallback), handler);  
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            var handler = (Socket) ar.AsyncState!;
            var bytesSent = handler.EndSend(ar);  
            _logger.Info($"Sent {bytesSent} bytes to client.");
            handler?.Shutdown(SocketShutdown.Both);  
            handler?.Close();
        }
        catch (Exception e)
        {
            _logger.Error("The message could not be send to the agent", e);  
        }  
    }
    
}