using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace PIT_Server
{
    public class ClientTCP : IDisposable
    {
        Socket _sock;
        IPEndPoint _endpoint;
        IMaster _callback;
        bool _disposed;
        object _synlock;

        public ClientTCP(string ip, int port, IMaster callback)
        {
            _endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _callback = callback;
            _disposed = false;
            _synlock = new object();
            _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect()
        {
            try
            {
                _sock.Connect(_endpoint);
            }
            catch (Exception e)
            {
                _callback.OnConnectError(e.Message);
            }
            if (_sock.Connected)
            {
                _callback.OnConnect("");
                Task.Factory.StartNew(() => _receive(), TaskCreationOptions.LongRunning);
                //Thread t = new Thread(new ThreadStart(_receive));
                //t.Start();
            }
        }
        public bool Connect(string ip, int port)
        {
            try
            {
                _sock.Connect(new IPEndPoint(IPAddress.Parse(ip),port));
            }
            catch (Exception e)
            {
                _callback.OnConnectError(e.Message);
                return false;
            }
            if (_sock.Connected)
            {
                _callback.OnConnect("");
                Task.Factory.StartNew(() => _receive(), TaskCreationOptions.LongRunning);
                //Thread t = new Thread(new ThreadStart(_receive));
                //t.Start();
                return true;
            }
            return false;
        }
        public void Send(string message)
        {
            if (_disposed)
                return;
            _send(Encoding.UTF8.GetBytes(message+"|"));
        }
        public void ChangeOwner(IMaster callback)
        {
            _callback = callback;
        }
        public void Dispose()
        {
            lock (_synlock)
            {
                if (_sock != null)
                {
                    _disposed = true;
                    _sock.Close();
                    _sock = null;
                }
            }
        }
        public void Disconnect()
        {
            Dispose();
        }
        private void _receive()
        {
            int _inbytes = 0;
            byte[] _buffer = new byte[1024];
            while (true)
            {
                if (_disposed)
                    return;
                try
                {
                    _inbytes = _sock.Receive(_buffer);
                }
                catch
                {
                    _callback.OnReceiveError("");
                    Dispose();
                    break;
                }
                if (_inbytes != 0)
                {
                    string message = Encoding.UTF8.GetString(_buffer,0,_inbytes);
                    string[] splited = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in splited)
                    {
                        string[] msg = s.Split(new char[] { '_' });
                        _callback.OnReceive(msg);
                    }
                }
            }
        }
        private void _send(byte[] msg)
        {
            try
            {
                _sock.Send(msg);
            }
            catch
            {
                Dispose();
                _callback.OnSendError("");
            }
        }
    }
    public interface IMaster
    {
        void OnConnect(string s);
        void OnReceive(string[] s);
        void OnConnectError(string s);
        void OnSendError(string s);
        void OnReceiveError(string s);
    }
    
}