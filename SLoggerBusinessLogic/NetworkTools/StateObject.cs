using System.Net.Sockets;
using System.Text;

namespace SLoggerBusinessLogic.NetworkTools;

public class StateObject
{
    public const int BufferSize = 1024;
    public readonly byte[] Buffer = new byte[BufferSize];
    public StringBuilder Sb = new StringBuilder();
    public Socket? WorkSocket = null;
}