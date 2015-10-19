using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.CommandQuery
{
    public class Authenticate: ICommand
    {
        private readonly string _username;
        private readonly string _password;

        public Authenticate(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public bool Execute(NntpClient context)
        {
            using (var authUserResponse = context.PerformRequest(new NntpRequest(NntpCommand.AuthUser, false, _username)))
            {
                if (authUserResponse.ResponseCode != NntpResponseCode.MoreAuthenticationInformationRequired)
                    return false;

                using (var authPassResponse = context.PerformRequest(new NntpRequest(NntpCommand.AuthPass, false, _password)))
                {
                    if (authPassResponse.ResponseCode != NntpResponseCode.AuthenticationAccepted)
                        return false;

                    return true;
                }
            }
        }
    }
}