using Experiment.Conversion;
using Experiment.NetworkIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Experiment.NetWorkIO
{
    public class Client
    {
        private ClientManager cm;
        private Socket socket;
        MemoryStream ms = new MemoryStream();
        public Client(ClientManager _cm, Socket _socket)
        {
            cm = _cm;
            socket = _socket;
        }
        public bool IsConneted = false;
        //입출력이 끊긴 경우의 처리는 대리자로 하고 미전송 데이터를 매개로 준다.

        //초기화하기는 했지만 호출할때는 반드시 null검사할것.
        public UnsentDataEvent unsentDataEvent = EmptyFuntions.EmptyFuntion;

        public IsSuccess Send(Convertible cv)
        {
            cv.Encode(ms);
            int n = socket.Send(ms.GetBuffer(), SocketFlags.None, out SocketError error);
            if (error == SocketError.Success)
            {
                return IsSuccess.Success;
            }
            else
            {
                unsentDataEvent(cv);
                return IsSuccess.failure;
            }
        }
        public IsSuccess Recieve(out Convertible cv)
        {
            Span<byte> buffer=new Span<byte>();
            socket.Receive(buffer, SocketFlags.None, out SocketError error);
            ms.Write(buffer);
            //20240417 여기서 마우리 
        }
    }
}
