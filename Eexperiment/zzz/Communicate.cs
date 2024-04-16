using System;
using static System.Console;
using Experiment;
using System.Net.Sockets;

namespace Experiment
{
    public delegate int SendFuntion(Span<byte> buff);
    public delegate int RecieveFuntion(Span<byte> buff);
    public class Communicate//데이터 주고받기와 인코딩 디코딩을 맡는다.
    {
        ClinetManager clinetManager;
        public Communicate(ClinetManager _clinetManager)
        {
            clinetManager = _clinetManager;
        }
        byte[] buff = new byte[1024];
        public void Processing()
        {
            var clinets=clinetManager.GetClinets();
            lock(clinets)
            {
                foreach(Socket s in clinets)
                {
                    int a = s.Receive(buff, SocketFlags.None, out SocketError sr);
                    switch(sr)
                    {
                        case SocketError.Success:
                            break;
                    }
                }
            }
        }
    }
}
