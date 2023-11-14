using SuperSimpleTcp;

namespace Journal.Models;

public static class Server
{
    public static SimpleTcpClient server;
    public static UserInfo user = new UserInfo();
    public static bool isConnected = false;
    public static string ip = "";
    public static string messageBuffer = "";
}