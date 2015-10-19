using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.CommandQuery
{
    public class SetModeReader: ICommand
    {
        public bool Execute(NntpClient context)
        {
            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.ModeReader, false)))
            {
                return response.ResponseCode == NntpResponseCode.ServerReadyPostingAllowed || response.ResponseCode == NntpResponseCode.ServerReadyNoPostingAllowed;
            }
        }
    }
}