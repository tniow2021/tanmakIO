using Conversion;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using static System.Console;

interface IWork
{
    abstract int Update();
}
static class Program
{
    static Task Accepting_Task;//접속허가
    static Task DataCommunicating_Task;//데이터 주고받기
    static Task processing_Task;//처리

    static bool exit = false;
    static void Main()
    {
        Accepting_Task = new Task(Accepting);
        DataCommunicating_Task = new Task(DataCommunicating);
        processing_Task = new Task(processing);

        Accepting_Task.Start();
        DataCommunicating_Task.Start();
        processing_Task.Start();

        Accepting_Task.Wait();
        DataCommunicating_Task.Wait();
        processing_Task.Wait();
    }

    static Harbor harbor;
    static void Accepting()
    {
        harbor = new Harbor(2024);
        while (exit is false)
        {
            harbor.Update();
        }
    }
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