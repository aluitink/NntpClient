using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class GetBody : IQuery<Body>
    {
        private readonly long _articleId;
        private readonly string _messageId;

        public GetBody(long articleId)
        {
            _articleId = articleId;
        }

        public GetBody(string messageId)
        {
            _messageId = messageId;
        }

        public Body Execute(NntpClient context)
        {
            var isArticleId = _articleId > 0;
            string param = isArticleId ? _articleId.ToString() : _messageId;

            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.Body, true, param)))
            {
                return new Body(response);
            }
        }
    }
}