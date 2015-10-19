using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Helvetica.Common.Logging;
using Helvetica.NntpClient.Interfaces;
using log4net;

namespace Helvetica.NntpClient.Models
{
    public class HeaderCollection: NntpModelBase
    {
        public string Name { get; private set; }
        public IEnumerable<KeyValuePair<long, string>> Articles { get { return GetPayload(); } }
        
        private string _tempFilePath;

        private static readonly ILog Logger = new Log("HeaderCollection");

        public HeaderCollection(NntpResponse response)
        {
            Parse(response);
        }

        protected override void Parse(NntpResponse response)
        {
            ThrowIfUnexpectedResponseCode(response, NntpResponseCode.ArticleRetrievedHeadFollows);

            var statusLine = response.StatusLine;

            var statusParts = statusLine.Split(new[] {' '}, 3);

            try
            {
                Name = statusParts[1];
            }
            catch (Exception)
            {
                Logger.WarnFormat("Could not parse status line '{0}'", statusLine);
            }

            _tempFilePath = GetOwnedPayload(response);
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
                Logger.WarnFormat("Could not delete temp file '{0}'", _tempFilePath);
            }
        }

        private IEnumerable<KeyValuePair<long, string>> GetPayload()
        {
            return LazyReadPayload(_tempFilePath, LineToKeyValuePair);
        }

        private KeyValuePair<long, string> LineToKeyValuePair(string line)
        {
            var parts = line.Split(new[] { ' ' }, 2);

            long articleId = 0;
            string articleIdStr = null;
            string valueStr = null;

            articleIdStr = parts[0];
            valueStr = parts[1];

            if (!string.IsNullOrWhiteSpace(articleIdStr))
                articleId = Convert.ToInt64(articleIdStr);

            return new KeyValuePair<long, string>(articleId, valueStr);
        }
    }
}
