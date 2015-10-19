using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class SetNext : ICommand
    {
        public Stat Stat { get; private set; }

        public bool Execute(NntpClient context)
        {
            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.Next, false)))
            {
                var success = response.ResponseCode == NntpResponseCode.ArticleRetrievedRequestTextSeparately;
                if (success)
                    Stat = new Stat(response);
                return success;    
            }
        }
    }
}