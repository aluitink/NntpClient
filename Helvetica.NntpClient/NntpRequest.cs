using System;
using System.Collections.Generic;
using System.IO;
using Helvetica.Common.Logging;

namespace Helvetica.NntpClient
{
    public class NntpRequest
    {
        public string Command { get; private set; }
        public List<string> Arguments { get; private set; }
        public Stream Payload { get; private set; }
        public bool CacheResponse { get; private set; }
        private static readonly Log _logger = new Log("NntpRequest");

        public NntpRequest(String command, bool cacheResponse, params string[] args)
        {
            _logger.DebugFormat("Enter NntpRequest({0})", command);
            CacheResponse = cacheResponse;
            Arguments = new List<string>(args);
            Command = command;
            _logger.DebugFormat("Leave NntpRequest");
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", Command, string.Join(" ", Arguments.ToArray()));
        }
    }
}