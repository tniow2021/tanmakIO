using Experiment.Conversion;
using System.Net.Sockets;
using Experiment.NetworkIO;
using Experiment.NetWorkIO;

namespace Experiment.NetworkIO
{
    /// <summary>
    /// 소켓을 저장하는 역할.
    /// 소켓의 생존여부를 판단하는 역할.
    /// 소켓이 생존하지않을때 자동으로 제거하는 역할.
    /// 소켓으로 데이터를 주고받는 역할.
    /// </summary>
    public class ClientManager
    {
        //&&& temp
        List<Client> clients = new List<Client>();
        public ClientManager(Greeter greeter)
        {
            greeter.connectEvent += connectEvent;
        }
        void connectEvent(Socket socket)
        {
            clients.Add(new Client(this,socket));
        }

        //&&& task와  타이머 이벤트로 소켓 살아있는 확인하는 코드짜기
    }
}
