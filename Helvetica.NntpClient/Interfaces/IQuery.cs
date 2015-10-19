namespace Helvetica.NntpClient.Interfaces
{
    public interface IQuery<T>
    {
        T Execute(NntpClient context);
    }
}