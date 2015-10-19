using System;
using System.IO;
using System.Text;
using Helvetica.Common.Logging;
using Helvetica.NntpClient.Interfaces;
using log4net;

namespace Helvetica.NntpClient
{
    public class NntpResponse: IDisposable
    {
        public NntpResponseCode ResponseCode { get; private set; }
        public string StatusLine { get; private set; }
        public Stream Payload { get { return GetPayloadStream(); } }

        private byte[] _payloadBytes;

        internal string TempFilePath;

        private static readonly ILog Logger = new Log("NntpResponse");

        public NntpResponse(string statusLine)
        {
            StatusLine = statusLine.TrimEnd('\r', '\n');//Get rid of the CrLf
            ResponseCode = ParseResponeCode(StatusLine);
        }

        public void HandlePayload(string payload, bool cacheToDisk)
        {
            _payloadBytes = Encoding.UTF8.GetBytes(payload);
            if (cacheToDisk)
            {
                TempFilePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(TempFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.Write(_payloadBytes, 0, _payloadBytes.Length);
                }
                _payloadBytes = null;
            }
        }

        public void Dispose()
        {
            try
            {
                if (File.Exists(TempFilePath))
                    File.Delete(TempFilePath);
            }
            catch (Exception)
            {
                Logger.WarnFormat("Could not delete temp file '{0}', please dispose the payload stream references before disposing the NntpResponse object.", TempFilePath);
            }
        }

        private NntpResponseCode ParseResponeCode(string line)
        {
            try
            {
                string code = line.Substring(0, 3);
                int codeVal = Convert.ToInt32(code);
                return (NntpResponseCode) codeVal;
            }
            catch (Exception e)
            {
                return NntpResponseCode.Unknown;
            }
        }

        //Creates an owned stream reference to temp file, caller is responsible for dispose.
        private Stream GetPayloadStream()
        {
            if(_payloadBytes != null)
                return new MemoryStream(_payloadBytes);
            if (TempFilePath == null || !File.Exists(TempFilePath))
                return null;
            return new FileStream(TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
        }   
    }
}