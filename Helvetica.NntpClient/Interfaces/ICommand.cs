namespace Helvetica.NntpClient.Interfaces
{
    public interface ICommand
    {
        bool Execute(NntpClient context);
    }
}