using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class GetHead : IQuery<Head>
    {
        private readonly long _articleId;
        private readonly string _messageId;

        public GetHead(long articleId)
        {
            _articleId = articleId;
        }

        public GetHead(string messageId)
        {
            _messageId = messageId;
        }

        public Head Execute(NntpClient context)
        {
            var isArticleId = _articleId > 0;
            string param = isArticleId ? _articleId.ToString() : _messageId;

            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.Head, false, param)))
            {
                return new Head(response);// response will set IsMissing if article is missing.
            }
        }
    }
}