using System;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.Models
{
    public class Stat : NntpModelBase
    {
        public long ArticleId { get; private set; }
        public string MessageId { get; private set; }

        public Stat(NntpResponse response)
        {
            Parse(response);
        }

        protected override void Parse(NntpResponse response)
        {
            ThrowIfUnexpectedResponseCode(response, NntpResponseCode.ArticleRetrievedRequestTextSeparately);
            
            string articleId = null;
            string messageId = null;
            
            try
            {
                string statusLine = response.StatusLine;

                var parts = statusLine.Split(new[] { ' ' }, 3);

                articleId = parts[1];
                messageId = parts[2];

            }
            catch (Exception)
            {
                //Likely index out of range.. ignore
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(articleId))
                    ArticleId = Convert.ToInt64(articleId);
                if (!string.IsNullOrWhiteSpace(messageId))
                    MessageId = messageId;
            }
        }
    }
}