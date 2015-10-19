using System.IO;
using Helvetica.NntpClient.Models;

namespace Helvetica.NntpClient.Interfaces
{
    public interface INntpClient
    {
        bool Authenticated { get; }
        long CurrentArticleId { get; }
        string CurrentMessageId { get; }
        Group CurrentGroup { get; }
        Article CurrentArticle { get; }
        string QuitMessage { get; }
        bool Connected { get; }
        Stream AccessStream { get; }
        void Connect();
        void Disconnect();
        bool Authenticate(string username, string password);
        bool Group(string group);
        bool Stat(long articleId);
        bool Next();
        bool Last();
        bool Quit();
        Article Article(long articleId);
        Article Article(string messageId);
        Body Body(long articleId);
        Body Body(string messageId);
        Head Head(long articleId);
        Head Head(string messageId);
        HeaderCollection Headers(string header, long articleIdStart, long articleIdEnd = 0);
        HeaderCollection Headers(string header, string messageId);
        GroupCollection List();
        T Query<T>(IQuery<T> query);
        bool Execute(ICommand command);
        void Dispose();
        void Reset();
    }
}