using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;
using NLog.Web;

namespace SLoggerBusinessLogic;

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
        var listener = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);
            while (true)
            {
                allDone.Reset();  
  
                // Start an asynchronous socket to listen for connections.  
                Console.WriteLine("Waiting for a connection...");  
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),  
                    listener );  
  
                // Wait until a connection is made before continuing.  
                allDone.WaitOne(); 
            }
        }
        catch (Exception e)
        {
            _logger.Error($"The Server could not start lisiting on port {_port}",e);
        }
    }
    public static void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.  
        allDone.Set();  
  
        // Get the socket that handles the client request.  
        var listener = (Socket) ar.AsyncState!;  
        var handler = listener?.EndAccept(ar);  
  
        // Create the state object.  
        var state = new StateObject
        {
            WorkSocket = handler
        };
        handler?.BeginReceive( state.Buffer, 0, StateObject.BufferSize, 0,  
            new AsyncCallback(ReadCallback), state);  
    }
    
    public static void ReadCallback(IAsyncResult ar)
    {
        var content = string.Empty;  
  
        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        var state = (StateObject) ar.AsyncState!;  
        var handler = state.WorkSocket;  
  
        // Read data from the client socket.
        if (handler != null)
        {
            var bytesRead = handler.EndReceive(ar);  
  
            if (bytesRead > 0) {
                state.Sb.Append(Encoding.ASCII.GetString(  
                    state.Buffer, 0, bytesRead));
                content = state.Sb.ToString();  
                if (content.IndexOf("<EOF>") > -1) {
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",  
                        content.Length, content );  
                    // Echo the data back to the client.  
                    Send(handler, content);  
                } else {
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,  
                        new AsyncCallback(ReadCallback), state);  
                }  
            }
        }
    }
    
    private static void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        var byteData = Encoding.ASCII.GetBytes(data);  
  
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
            Console.WriteLine(e.ToString());  
        }  
    }
    
}