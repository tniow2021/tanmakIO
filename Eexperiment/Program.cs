using Conversion;
using static System.Console;


namespace Eexperiment
{
    static class Program
    {
        static Harbor harbor;
        static ClinetManager clinetManager;

        static Task<int> Data_Communicating_And_Processing;
        public static int maxThreadNumber {  get;private set; }
        public static int port { get; private set; }
        static void Main()
        {
            while (true)
            {
                try 
                { 
                    WriteLine("Enter the maximum number of processors on this computer.");
                    string t = ReadLine()+"";
                    maxThreadNumber = Convert.ToInt32(t);

                    WriteLine("Port number setting.");
                    t = ReadLine() + "";
                    port = Convert.ToInt32(t);
                    if(port<=0||port>short.MaxValue)
                    {
                        WriteLine("error: Port number is out of range. Try again.");
                        continue;
                    }
                }
                catch(Exception e)
                {
                    WriteLine(e.ToString());
                    WriteLine("Enter again");
                    continue;
                }
                WriteLine($"Is {maxThreadNumber}, {port} what you entered ? (yn)");
                string tt = ReadLine()+"";
                if(tt!="y")
                {
                    WriteLine("try again");
                }
                else
                {
                    break;
                }
            }
            WriteLine("start");
            try
            {
                clinetManager = new ClinetManager();
                harbor = new Harbor((short)port);
                harbor.AcceptingStart();
            }
            catch (Exception e)
            {
                WriteLine("error");
                WriteLine(e.ToString());
                WriteLine("Restart the program");
                return;
            }

            Data_Communicating_And_Processing = new Task<int>(DCP);
            Thread.Sleep(90000000);
        }
        static int DCP()
        {
            return 0;
        }
    }
}


/*
 * 
 * 
 * 병렬작업을 할때 cpu집약적이지 않은 것은 논 블로킹과 task를 이용하라
 * 동시에 여러개의 task가 만들어지지 않게 해라 
 * 단발로 끝나는 작은 작업들은 대리자 큐를 만들어서 cpu쓰레드갯수만큼의 task를 한번에 처리하라 
 * 
 * task객체 는 이미 있는 쓰레드풀을 이용하기 때문에 new task의 오버헤드는 크지않다.
 * 이 오버헤드가 걱정된다면 미리 task 풀을 만드는 것도 방법,
 * 
 * 작업의 성격별로 미리 task를 만들어서 while문으로다 돌리는 것보다 task풀을 먼저 만든뒤,
 *  그때 그때 task를 땡겨쓰거나 아예 그떄그때 task를 만들던가 asyn wait 뭐시기를 쓰는게 좋다.
 *  다만 동시에 여러개의 블로킹이 생기게 하지마라
 */