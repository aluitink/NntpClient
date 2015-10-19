using System;
using Helvetica.Common.Logging;
using Helvetica.NntpClient.CommandQuery;
using Helvetica.NntpClient.Interfaces;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient
{
    public class NntpClient : Client, IContext, INntpClient
    {
        public bool Authenticated { get; private set; }

        public long CurrentArticleId { get; private set; }
        public string CurrentMessageId { get; private set; }
        public Group CurrentGroup { get; private set; }
        public Article CurrentArticle { get { return Article(CurrentArticleId); } }

        public string QuitMessage { get; private set; }

        protected virtual NntpProtocolHandler ProtocolHandler { get; private set; }

        private static readonly Log Logger = new Log("NntpClient");

        private string _username;
        private string _password;

        public NntpClient(string host, int port, bool useSsl, ITcpClient client = null) 
            : base(host, port, useSsl, client)
        {
            if (Logger != null)
            {
                Logger.DebugFormat("Enter NntpClient({0}, {1}, {2})", host, port, useSsl);

                Logger.DebugFormat("Leave NntpClient");
            }
        }

        public override void Connect()
        {
            Logger.DebugFormat("Enter Connect()");
            base.Connect();
            InitializeStream();
            Logger.DebugFormat("Leave Connect");
        }

        public override void Disconnect()
        {
            Authenticated = false;
            base.Disconnect();
        }

        public bool Authenticate(string username, string password)
        {
            Authenticated = Execute(new Authenticate(username, password));
            if (Authenticated)
            {
                _username = username; //Store for reconnect
                _password = password;
            }
            return Authenticated;
        }

        public bool Group(string group)
        {
            var setGroup = new SetGroup(group);
            var success = Execute(setGroup);
            if (success)
            {
                CurrentGroup = setGroup.Group;
                CurrentArticleId = 0;
                CurrentMessageId = null;
            }
            return success;
        }

        public bool Stat(long articleId)
        {
            var setStat = new SetStat(articleId);
            var success = Execute(setStat);
            if (success)
                SetCurrentStat(setStat.Stat);
            return success;
        }

        public bool Next()
        {
            var setNext = new SetNext();
            var success = Execute(setNext);
            if (success)
                SetCurrentStat(setNext.Stat);
            return success;
        }

        public bool Last()
        {
            var setLast = new SetLast();
            var success = Execute(setLast);
            if(success)
                SetCurrentStat(setLast.Stat);
            return success;
        }

        public bool Quit()
        {
            var setQuit = new SetQuit();
            var success = Execute(setQuit);
            if (success)
                QuitMessage = setQuit.Message;
            return success;
        }

        public Article Article(long articleId)
        {
            return Query(new GetArticle(articleId));
        }

        public Article Article(string messageId)
        {
            return Query(new GetArticle(messageId));
        }

        public Body Body(long articleId)
        {
            return Query(new GetBody(articleId));
        }

        public Body Body(string messageId)
        {
            return Query(new GetBody(messageId));
        }

        public Head Head(long articleId)
        {
            return Query(new GetHead(articleId));
        }

        public Head Head(string messageId)
        {
            return Query(new GetHead(messageId));
        }

        public HeaderCollection Headers(string header, long articleIdStart, long articleIdEnd = 0)
        {
            return Query(new GetHeaders(header, articleIdStart, articleIdEnd));
        }

        public HeaderCollection Headers(string header, string messageId)
        {
            return Query(new GetHeaders(header, messageId));
        }

        public GroupCollection List()
        {
            return Query(new GetGroups());
        }

        public T Query<T>(IQuery<T> query)
        {
            return query.Execute(this);
        }

        public bool Execute(ICommand command)
        {
            return command.Execute(this);
        }

        public override void Dispose()
        {
            if(ProtocolHandler != null)
                ProtocolHandler.Dispose();
            base.Dispose();
        }

        protected virtual void SetCurrentStat(Stat stat)
        {
            CurrentArticleId = stat.ArticleId;
            CurrentMessageId = stat.MessageId;
        }

        internal NntpResponse PerformRequest(NntpRequest request)
        {
            try
            {
                return ProtocolHandler.PerformRequest(request);
            }
            catch (Exception ex)
            {
                Logger.Error("Could not perform request", ex);
                Disconnect();
                Connect();
                Authenticate(_username, _password);
                if (request.Command != NntpCommand.Group && CurrentGroup != null)
                {
                    if (Execute(new SetGroup(CurrentGroup.Name)))
                    {
                        return PerformRequest(request);
                    }
                }
                return PerformRequest(request);
            }
        }

        private void InitializeStream()
        {
            Logger.DebugFormat("Enter InitializeStream()");
            ProtocolHandler = new NntpProtocolHandler(new StreamAccessor(AccessStream));
            Logger.DebugFormat("Leave InitializeStream");
        }
    }
}