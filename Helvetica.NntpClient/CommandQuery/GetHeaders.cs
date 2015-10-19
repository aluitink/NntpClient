using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class GetHeaders: IQuery<HeaderCollection>
    {
        private readonly string _header;
        private readonly string _messageId;
        private readonly long _articleIdStart;
        private readonly long _articleIdEnd;
        
        public GetHeaders(string header, long articleId)
        {
            _header = header;
            _articleIdStart = articleId;
        }

        public GetHeaders(string header, long articleIdStart, long articleIdEnd = -1)
        {
            _header = header;
            _articleIdStart = articleIdStart;
            _articleIdEnd = articleIdEnd;
        }

        public GetHeaders(string header, string messageId)
        {
            _header = header;
            _messageId = messageId;
        }

        public HeaderCollection Execute(NntpClient context)
        {
            var isArticleId = _articleIdStart > 0;
            var isRangeQuery = _articleIdEnd > 0;
            var isOpenRangeQuery = _articleIdEnd == -1;

            string param = string.Empty;
            if (!isArticleId)
            {
                param = _messageId;
            }
            else if(isOpenRangeQuery)
            {
                param = string.Format("{0}-", _articleIdStart);
            }
            else if (isRangeQuery)
            {
                param = string.Format("{0}-{1}", _articleIdStart, _articleIdEnd);
            }

            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.XHdr, true, _header, param)))
            {
                var success = response.ResponseCode == NntpResponseCode.ArticleRetrievedHeadFollows;

                return success ? new HeaderCollection(response) : null;
            }
        }
    }
}
