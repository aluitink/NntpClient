using System.IO;

namespace Helvetica.NntpClient.Interfaces
{
    public interface ITcpClient
    {
        void Connect(string host, int port);
        void Close();
        Stream GetStream();
    }
}