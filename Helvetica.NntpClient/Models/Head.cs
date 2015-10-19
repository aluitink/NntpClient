using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.Models
{
    public class Head : ArticlePartBase
    {
        public ReadOnlyDictionary<string, string> Headers { get { return _headers; } }

        private ReadOnlyDictionary<string, string> _headers; 

        public Head(NntpResponse response)
        {
            Parse(response);
        }

        protected override void Parse(NntpResponse response)
        {
            base.Parse(response);
            switch (response.ResponseCode)
            {
                case NntpResponseCode.ArticleRetrievedHeadAndBodyFollows:
                    HandleHeadersAndBody(response);
                    break;
                case NntpResponseCode.ArticleRetrievedHeadFollows:
                    HandleHeaders(response);
                    break;
            }
        }

        private void HandleHeadersAndBody(NntpResponse response)
        {
            using (var streamReader = new StreamReader(response.Payload, new UTF8Encoding(false)))
            {
                var headers = new Dictionary<string, string>();
                var line = streamReader.ReadLine();

                while (line != null && !line.Equals(HeaderBodyDelimiter))
                {
                    var parts = line.Split(new[] {':'}, 2);
                    var key = parts[0];
                    var value = parts[1].TrimStart(' ');

                    headers.Add(key, value);
                    line = streamReader.ReadLine();
                }

                _headers = new ReadOnlyDictionary<string, string>(headers);
            }
        }

        private void HandleHeaders(NntpResponse response)
        {
            using (var streamReader = new StreamReader(response.Payload, new UTF8Encoding(false)))
            {
                var headers = new Dictionary<string, string>();
                var line = streamReader.ReadLine();
                var previousKey = string.Empty;
                while (line != null && !line.Equals(BlockTerminator))
                {
                    if (line.StartsWith("\t") || line.StartsWith(" "))//line continuation
                    {
                        string val = headers[previousKey];
                        val = val + line.TrimStart('\t');
                        headers[previousKey] = val;
                    }
                    else
                    {
                        var parts = line.Split(new[] {':'}, 2);
                        var key = parts[0];
                        previousKey = key;
                        var value = parts[1].TrimStart(' ');

                        try
                        {
                            headers.Add(key, value);
                        }
                        catch (ArgumentException ex)
                        {
                            //if key was already there.. concat the values
                            var val1 = headers[key];
                            var newVal = string.Format("{0} {1}", val1, value);
                            headers[key] = newVal;
                        }
                    }

                    line = streamReader.ReadLine();

                }

                _headers = new ReadOnlyDictionary<string, string>(headers);
            }
        }
    }
}