static class Program
{
    static void Main()
    {
        server myServer=new server();
        while(true)
        {
            myServer.Update();
            //접속자가 없으면 스레드 슬립으로 100ms 단위로 자고있게 하면 좋을듯
        }
    }
}
class server
{
    //bind해서 접속자 받는 소켓은 하나
    //클라이언트들을 관리하기 위해 클라이언트 소켓들 관리할 구조가 필요.
    //일단 기본적으로 리스트만.

    List <Socket> clients=new List<Socket>();
    public void Update()
    {

    }
}