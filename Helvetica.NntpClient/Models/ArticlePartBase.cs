using System;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.Models
{
    public abstract class ArticlePartBase : NntpModelBase
    {
        public bool IsMissing { get; private set; }
        public long ArticleId { get; private set; }
        public string MessageId { get; private set; }

        protected const string HeaderBodyDelimiter = "";

        protected override void Parse(NntpResponse response)
        {
            try
            {
                ThrowIfUnexpectedResponseCode(response,
                NntpResponseCode.ArticleRetrievedHeadFollows,
                NntpResponseCode.ArticleRetrievedHeadAndBodyFollows,
                NntpResponseCode.ArticleRetrievedBodyFollows);

                string articleId = null;
                string messageId = null;

                try
                {
                    string statusLine = response.StatusLine;

                    var parts = statusLine.Split(new[] { ' ' }, 4);

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
            catch (Exception)
            {
                IsMissing = true;
            }
        }
    }
}