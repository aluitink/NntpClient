using System;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.Models
{
    public class Group: NntpModelBase
    {
        public string Name { get; private set; }
        public long Count { get; private set; }
        public long First { get; private set; }
        public long Last { get; private set; }
        public bool? CanPost { get; private set; }

        public Group(NntpResponse response)
        {
            Parse(response);
        }

        internal Group(string name, long first, long last, bool canPost)
        {
            Name = name;
            First = first;
            Last = last;
            CanPost = canPost;

            if (last > first)
                Count = last - first;
        }

        protected override void Parse(NntpResponse response)
        {
            ThrowIfUnexpectedResponseCode(response, NntpResponseCode.NflsGroupSelected);

            string count = null;
            string first = null;
            string last = null;
            string name = null;

            try
            {
                string statusLine = response.StatusLine;

                var parts = statusLine.Split(' ');

                count = parts[1];
                first = parts[2];
                last = parts[3];
                name = parts[4];
            }
            catch (Exception)
            {
                //Likely index out of range.. ignore
            }
            finally
            {
                Name = name;
                if(count != null)
                    Count = Convert.ToInt64(count);
                if(first != null)
                    First = Convert.ToInt64(first);
                if(last != null)
                    Last = Convert.ToInt64(last);
            }
        }
    }
}