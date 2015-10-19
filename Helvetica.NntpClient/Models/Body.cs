using System;
using System.IO;
using System.Text;
using Helvetica.Common.Logging;
using Helvetica.NntpClient.Interfaces;
using log4net;

namespace Helvetica.NntpClient.Models
{
    public class Body : ArticlePartBase
    {
        public Stream Data { get { return GetPayloadStream(); } }

        private string _tempFilePath;

        private static readonly ILog Logger = new Log("Body");

        public Body(NntpResponse response)
        {
            Parse(response);
        }

        public override void Dispose()
        {
            try
            {
                if (File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }
            catch (Exception)
            {
                Logger.WarnFormat("Could not delete temp file '{0}', please dispose the payload stream references before disposing the Body object.", _tempFilePath);
            }
        }

        protected override void Parse(NntpResponse response)
        {
            base.Parse(response);
            switch (response.ResponseCode)
            {
                case NntpResponseCode.ArticleRetrievedHeadAndBodyFollows:
                    HandleHeadersAndBody(response);
                    break;
                case NntpResponseCode.ArticleRetrievedBodyFollows:
                    HandleBody(response);
                    break;
            }
        }

        private void HandleHeadersAndBody(NntpResponse response)
        {
            using (var streamReader = new StreamReader(response.Payload, new UTF8Encoding(false)))
            {
                _tempFilePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(_tempFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var line = streamReader.ReadLine();
                    bool isInBody = false;
                    while (line != null && !line.Equals(BlockTerminator))
                    {
                        if (isInBody)
                        {
                            var lineBytes = Encoding.UTF8.GetBytes(line);
                            fileStream.Write(lineBytes, 0, lineBytes.Length);
                        }

                        if (line.Equals(HeaderBodyDelimiter))
                            isInBody = true;
                        line = streamReader.ReadLine();
                    }
                }
            }
        }

        private void HandleBody(NntpResponse response)
        {
            using (var streamReader = new StreamReader(response.Payload, new UTF8Encoding(false)))
            {
                _tempFilePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(_tempFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    var line = streamReader.ReadLine();
                    while (line != null && !line.Equals(BlockTerminator))
                    {
                        var lineBytes = Encoding.UTF8.GetBytes(line);
                        fileStream.Write(lineBytes, 0, lineBytes.Length);
                        line = streamReader.ReadLine();
                    }
                }
            }
        }

        //Creates an owned stream reference to temp file, caller is responsible for dispose.
        private Stream GetPayloadStream()
        {
            if (_tempFilePath == null || !File.Exists(_tempFilePath))
                return null;
            return new FileStream(_tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
        }
    }
}