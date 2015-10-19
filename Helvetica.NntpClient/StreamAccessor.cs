using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Helvetica.NntpClient.Interfaces;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace Helvetica.NntpClient
{
    //public class CachedStream: FileStream
    //{
    //    public CancellationTokenSource CancellationTokenSource { get { return _cancellationTokenSource; } }

    //    private readonly Thread _readThread;
    //    private readonly Stream _stream;
    //    private readonly string _cacheFile;

    //    private readonly CancellationTokenSource _cancellationTokenSource;

    //    public CachedStream(Stream stream, string cacheFile, CancellationTokenSource cancellationTokenSource = null)
    //        : base(cacheFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose)
    //    {
    //        _stream = stream;
    //        _cacheFile = cacheFile;
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        int bytesRead = _stream.Read(buffer, offset, count);
    //        base.Write(buffer, );
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {

    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (_stream != null)
    //        {
    //            _stream.Dispose();
    //        }
    //    }
    //}

    public class StreamAccessor : IStreamAccessor
    {
        public virtual Stream BaseStream { get { return _baseStream; } }

        public virtual bool AutoFlush { get { return _streamWriter.AutoFlush; } set { _streamWriter.AutoFlush = value; } }
        public virtual string NewLine { get { return _streamWriter.NewLine; } set { _streamWriter.NewLine = value; } }
        
        private readonly Stream _baseStream;
        private readonly StreamReader _streamReader;
        private readonly StreamWriter _streamWriter;

        public StreamAccessor(Stream stream, Encoding encoding = null)
        {
            _baseStream = stream;
            _streamReader = new StreamReader(_baseStream, encoding ?? new UTF8Encoding(false));
            _streamWriter = new StreamWriter(_baseStream, encoding ?? new UTF8Encoding(false));
        }

        public virtual string ReadLine()
        {
            return _streamReader.ReadLine();
        }

        public virtual void WriteLine(string line)
        {
            _streamWriter.WriteLine(line);
        }

        public void Dispose()
        {
            if(_streamReader != null)
                _streamReader.Dispose();
            if(_streamWriter != null)
                _streamWriter.Dispose();
        }
    }
}