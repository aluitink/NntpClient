using System.IO;
using System.Net.Sockets;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient
{
    public class TcpClientWrapper: TcpClient, ITcpClient
    {
        public new void Connect(string host, int port)
        {
            base.Connect(host, port);
        }

        public new void Close()
        {
            base.Close();
        }
        public new Stream GetStream()
        {
            return base.GetStream();
        }
        
    }
}