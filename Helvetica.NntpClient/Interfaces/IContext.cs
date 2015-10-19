namespace Helvetica.NntpClient.Interfaces
{
    public interface IContext
    {
        T Query<T>(IQuery<T> query);
        bool Execute(ICommand command);
    }
}