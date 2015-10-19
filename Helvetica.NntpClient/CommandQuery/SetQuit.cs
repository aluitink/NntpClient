using System;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.CommandQuery
{
    public class SetQuit : ICommand
    {
        public string Message { get; private set; }

        public bool Execute(NntpClient context)
        {
            using (var response = context.PerformRequest(new NntpRequest(NntpCommand.Quit, false)))
            {
                var success = response.ResponseCode == NntpResponseCode.ClosingConnection;
                if (success)
                {
                    try
                    {
                        var statusLine = response.StatusLine;
                        var parts = statusLine.Split(new[] { ' ' }, 2);

                        Message = parts[1];
                    }
                    catch (Exception)
                    {
                        //Ignore parse errors, server is what matters
                    }
                }
                return success;    
            }
        }
    }
}