using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class GetArticle : IQuery<Article>
    {
        private readonly long _articleId;
        private readonly string _messageId;

        public GetArticle(long articleId)
        {
            _articleId = articleId;
        }

        public GetArticle(string messageId)
        {
            _messageId = messageId;
        }

        public Article Execute(NntpClient context)
        {
            var isArticleId = _articleId > 0;
            string param = isArticleId ? _articleId.ToString() : _messageId;

            using(var response = context.PerformRequest(new NntpRequest(NntpCommand.Article, true, param)))
            {
                return new Article(response);
            }
        }
    }
}