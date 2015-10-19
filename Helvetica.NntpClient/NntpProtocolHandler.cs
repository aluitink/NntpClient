using System;
using System.Text;
using System.Threading;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient
{
    public class NntpProtocolHandler : IDisposable
    {
        protected const string CrLf = "\r\n";
        protected const string BlockTerminator = ".";
        
        private readonly IStreamAccessor _streamAccessor;

        private readonly int _readTimeoutMsec;
        private readonly bool _streamTimeoutSupport;

        public NntpProtocolHandler(IStreamAccessor streamAccessor, int readTimeoutMsec = 15000)
        {
            _streamAccessor = streamAccessor;
            _streamAccessor.AutoFlush = true;
            _streamAccessor.NewLine = CrLf;

            _readTimeoutMsec = readTimeoutMsec;

            try
            {
                streamAccessor.BaseStream.ReadTimeout = _readTimeoutMsec;
                _streamTimeoutSupport = true;
            }
            catch (InvalidOperationException)
            {
                _streamTimeoutSupport = false;
            }

            var response = new NntpResponse(ReadLine());
            if (response.ResponseCode != NntpResponseCode.ServerReadyNoPostingAllowed && response.ResponseCode != NntpResponseCode.ServerReadyPostingAllowed)
                throw new ApplicationException("Unexpected Response Code");
        }

        public NntpResponse PerformRequest(NntpRequest request)
        {
            WriteLine(request.ToString());
            var statusLine = ReadLine();
            var response = new NntpResponse(statusLine);

            switch (response.ResponseCode)
            {
                case NntpResponseCode.Unknown://If unknown, we didn't get an expected status line, read to end, client should retry

                    int bytesToRead = (int)_streamAccessor.BaseStream.Length - (int)_streamAccessor.BaseStream.Position;
                    var garbage = new byte[bytesToRead];
                    var bytesRead = _streamAccessor.BaseStream.Read(garbage, 0, bytesToRead);
                    break;
                case NntpResponseCode.HelpTextFollows:
                case NntpResponseCode.ListOfNewArticlesByMessageIdFollows:
                case NntpResponseCode.ListOfNewNewsgroupsFollows:
                case NntpResponseCode.ListOfNewsgroupsFollows:
                case NntpResponseCode.ArticleRetrievedBodyFollows:
                case NntpResponseCode.ArticleRetrievedHeadFollows:
                case NntpResponseCode.ArticleRetrievedHeadAndBodyFollows:
                    response.HandlePayload(ReadBlock(), request.CacheResponse);
                    break;
            }
            return response;
        }

        protected string ReadLine()
        {
            var stringBuilder = new StringBuilder();
            var line = SafeReadLine();
            stringBuilder.AppendLine(line);
            return stringBuilder.ToString();
        }

        protected string ReadBlock()
        {
            var stringBuilder = new StringBuilder();

            string line;
            do
            {
                line = SafeReadLine();
                stringBuilder.AppendLine(line);
            } while (line != null && !line.Equals(BlockTerminator));
            return stringBuilder.ToString();
        }

        protected void WriteLine(string data)
        {
            _streamAccessor.WriteLine(data);
        }

        protected T CallWithTimeout<T>(Func<T> funcToTry, int timeoutMilliseconds)
        {
            if (funcToTry == null)
                throw new ArgumentNullException("funcToTry"); // slightly safer...

            Thread threadToKill = null;
            Func<T> wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                return funcToTry();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                return wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        private string SafeReadLine()
        {
            return _streamTimeoutSupport ? _streamAccessor.ReadLine() : CallWithTimeout(_streamAccessor.ReadLine, _readTimeoutMsec);
        }

        public void Dispose()
        {
            if(_streamAccessor != null)
                _streamAccessor.Dispose();
        }
    }
}
