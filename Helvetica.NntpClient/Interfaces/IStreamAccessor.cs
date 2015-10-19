using System;
using System.IO;

namespace Helvetica.NntpClient.Interfaces
{
    public interface IStreamAccessor: IDisposable
    {
        bool AutoFlush { get; set; }
        string NewLine { get; set; }

        Stream BaseStream { get; }

        string ReadLine();
        void WriteLine(string line);
    }
}