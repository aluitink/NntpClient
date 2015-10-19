using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.Models
{
    public abstract class NntpModelBase : IDisposable
    {
        protected const string BlockTerminator = ".";
        protected abstract void Parse(NntpResponse response);

        protected virtual string GetOwnedPayload(NntpResponse response)
        {
            if (!File.Exists(response.TempFilePath))
                throw new ApplicationException("Could not find temp file!");

            var newFile = Path.GetTempFileName();

            File.Copy(response.TempFilePath, newFile, true);
            return newFile;
        }

        protected virtual IEnumerable<T> LazyReadPayload<T>(string tempFilePath, Func<string, T> transformFunc)
        {
            using (var streamReader = new StreamReader(new FileStream(tempFilePath, FileMode.Open, FileAccess.Read), new UTF8Encoding(false)))
            {
                var line = streamReader.ReadLine();

                while (line != null && !line.Equals(BlockTerminator))
                {

                    T record = default(T);
                    try
                    {
                        record = transformFunc(line);
                    }
                    catch (Exception)
                    {
                        line = streamReader.ReadLine();
                        continue;
                    }

                    yield return record;
                    line = streamReader.ReadLine();
                }
            }
        }

        protected virtual void ThrowIfUnexpectedResponseCode(NntpResponse response, params NntpResponseCode[] expected)
        {
            if(!expected.Any(e => e.Equals(response.ResponseCode)))
                throw new ApplicationException("Unexpected Response From Server.");
        }

        public virtual void Dispose() { }
    }
}