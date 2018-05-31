using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
namespace PIT_Server
{
    public class Server
    {
        #region Vars
        IPEndPoint EPoint;
        #endregion
        public Server( int port)
        {
            EPoint = new IPEndPoint(IPAddress.Any,port);
        }
        public void Start()
        {
            Task.Factory.StartNew(() => _start()).Wait();
        }
        void _start()
        {
            using (Socket Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                Logger.LogIt(typeof(Server), "Server Listening ...");
                Listener.Bind(EPoint);
                Listener.Listen(1000);
                ClientAccept(Listener);
            }
        }
        void ClientAccept(Socket Listener)
        {
            while (true)
            {
                try
                {
                    VisitorBase.NewVisitor(Listener.Accept());
                }
                catch(Exception e)
                {
                    Logger.LogIt(typeof(Server), "Client accept error:" + e.Message);
                    break;
                }
            }
        }

    }
}
