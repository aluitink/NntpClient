using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.CommandQuery
{
    public class SetGroup: ICommand
    {
        public Group Group { get; private set; }

        private readonly string _group;

        public SetGroup(string group)
        {
            _group = group;
        }

        public bool Execute(NntpClient context)
        {
            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.Group, false, _group)))
            {
                var success = response.ResponseCode == NntpResponseCode.NflsGroupSelected;
                if (success)
                    Group = new Group(response);
                return success;    
            }
        }
    }
}