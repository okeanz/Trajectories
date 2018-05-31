using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace PIT_Server
{
    public partial class Agent : IDisposable
    {
        protected Socket _sock;
        protected IPEndPoint _endpoint;
        protected bool _disposed;
        protected object _synlock;
        public string Name;
        public Agent(Socket sock)
        {
            _endpoint = null;
            _sock = sock;
            _disposed = false;
            _synlock = new object();
            Name = Namer.GetRandomName();
        }
        public void Start()
        {
            Task.Factory.StartNew(() => _receive(), TaskCreationOptions.LongRunning);
        }
        public void Send(string message)
        {
            if (_disposed)
                return;
            message = message.Replace(',', '.');
            Logger.LogIt(typeof(Agent), "To:" + Name + ": " + message);
            _send(Encoding.UTF8.GetBytes(message+"|"));
        }
        public void SendClear(string message)
        {
            if (_disposed)
                return;
            if (PSettings.Debug)
            {
                Send(message);
                return;
            }
            message = message.Replace(',', '.');
            _send(Encoding.UTF8.GetBytes(message + "|"));
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
        private void _receive()
        {
            int _inbytes = 0;
            byte[] _buffer = new byte[4096];
            while (true)
            {
                if (_disposed)
                    return;
                try
                {
                    _inbytes = _sock.Receive(_buffer,0,_buffer.Length,SocketFlags.None);
                }
                catch
                {
                    if (!_disposed)
                    {
                        Communicator.OnError(this);
                        Dispose();
                        return;
                    }
                    else
                        return;
                }
                if (_inbytes != 0)
                {
                    string message = Encoding.UTF8.GetString(_buffer,0,_inbytes);
                    Logger.LogIt(typeof(Agent), "Get " + Name+":"+message);
                    message = message.Replace('.', ',');
                    string[] splited = message.Split(new char[] { '|' },StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in splited)
                    {
                        string[] msg = s.Split(new char[] { '_' });
                        try
                        {
                            Communicator.NewMessage(this, msg);
                        }
                        catch (Exception e)
                        {
                            Logger.LogIt(typeof(Agent), "Agent:" + Name + " Error:" + e.ToString());
                            Communicator.OnError(this);
                            Dispose();
                        }
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
                Communicator.OnError(this);
                Dispose();
            }
        }

    }
}