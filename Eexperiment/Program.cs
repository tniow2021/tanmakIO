using Conversion;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using static System.Console;


static class Program
{

    static bool exit = false;
    static void Main()
    {
        Console.WriteLine(5);
    }

    static Harbor harbor;
    static void DataCommunicating()
    {
        //while (exit is false)
        //{  
        //}
    }
    static void processing()
    {
        //while (exit is false)
        //{
        //}
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