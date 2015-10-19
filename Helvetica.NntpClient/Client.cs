using System;
using System.Threading;
using Helvetica.Common.Logging;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient
{
    public class Client: IDisposable
    {        
        public bool Connected { get; private set; }
        public virtual Stream AccessStream { get; private set; }

        protected ITcpClient TcpClient { get; private set; }
        protected SslStream SslStream { get; private set; }

        protected string Host { get; private set; }
        protected int Port { get; private set; }
        protected bool UseSsl { get; private set; }

        private static readonly Log Logger = new Log("Client");

        public Client(string host, int port, bool useSsl, ITcpClient client = null)
        {
            Logger.DebugFormat("Enter Client({0}, {1}, {2})", host, port, useSsl);
           
            TcpClient = client ?? new TcpClientWrapper();

            Host = host;
            Port = port;
            UseSsl = useSsl;

            Logger.DebugFormat("Leave Client");
        }

        public virtual void Connect()
        {
            Logger.DebugFormat("Enter Connect()");
            Logger.DebugFormat("Connected: {0}", Connected);
            if (!Connected)
            {
                Logger.DebugFormat("UseSsl: {0}", UseSsl);
                Logger.DebugFormat("TcpClient.Connect({0}, {1})", Host, Port);
                if(TcpClient == null)
                    TcpClient = new TcpClientWrapper();

                try
                {
                    RetryAction(() => TcpClient.Connect(Host, Port), 10, 10000, "Failed to Connect"); // try to connect, 10 times, waiting 10 seconds between attempts
                }
                catch (Exception ex) 
                {
                    Logger.Error(ex.Message, ex);
                }
                
                if (UseSsl)
                {
                    SslStream = new SslStream(TcpClient.GetStream());

                    Logger.DebugFormat("AuthenticateAsClient({0})", Host);
                    SslStream.AuthenticateAsClient(Host);

                    AccessStream = SslStream;
                }
                else
                {
                    AccessStream = TcpClient.GetStream();
                }
                Connected = true;
            }
            Logger.DebugFormat("Leave Connect");
        }

        public virtual void Disconnect()
        {
            Logger.DebugFormat("Enter Disconnect()");
            TcpClient.Close();
            TcpClient = null;
            Dispose();//Dispose streams
            Connected = false;
            Logger.DebugFormat("Leave Disconnect");
        }

        public virtual void Reset()
        {
            Logger.DebugFormat("Enter Rest()");
            Disconnect();
            Logger.DebugFormat("Leave Reset");
        }

        public virtual void Dispose()
        {
            if(SslStream != null)
                SslStream.Dispose();
            if(AccessStream != null)
                AccessStream.Dispose();
        }

        public void RetryAction(Action funcToTry, int numRetries, int retryTimeout, string failedMessage)
        {
            if (funcToTry == null)
                throw new ArgumentNullException("funcToTry"); // slightly safer...
            do
            {
                try
                {
                    funcToTry();
                    return;
                }
                catch
                {
                    Logger.Warn(failedMessage);
                    if (numRetries <= 0) throw;  // improved to avoid silent failure
                    Thread.Sleep(retryTimeout);
                }
            } while (numRetries-- > 0);
            throw new ApplicationException("Retries exhausted.");
        }

    }
}
