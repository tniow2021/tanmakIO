using Experiment.Conversion;
using System.Net.Sockets;

namespace Experiment.NetworkIO
{
    public delegate void TakeData(Convertible cb);
    public delegate void SocketConnectEvent(Socket socket);
    public delegate void UnsentDataEvent(Convertible unsentData);
}
