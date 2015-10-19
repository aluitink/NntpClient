using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class SetStat : ICommand
    {
        public Stat Stat { get; private set; }

        private readonly long _articleId;

        public SetStat(long articleId)
        {
            _articleId = articleId;
        }

        public bool Execute(NntpClient context)
        {
            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.Stat, false, _articleId.ToString())))
            {
                var success = response.ResponseCode == NntpResponseCode.ArticleRetrievedRequestTextSeparately;
                if (success)
                    Stat = new Stat(response);
                return success;    
            }
        }
    }
}