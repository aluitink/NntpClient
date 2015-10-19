using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class GetGroups : IQuery<GroupCollection>
    {
        public GroupCollection Execute(NntpClient context)
        {
            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.List, true)))
            {
                var success = response.ResponseCode == NntpResponseCode.ListOfNewsgroupsFollows;

                return success ? new GroupCollection(response) : null;
            }
        }
    }
}