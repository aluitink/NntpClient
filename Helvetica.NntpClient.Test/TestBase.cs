using System;
using System.IO;
using System.Reflection;
using System.Text;
using Helvetica.NntpClient.Interfaces;

namespace Helvetica.NntpClient.Test
{
    public class TestBase
    {
        public Stream GetResponseResources(string response, string payload = null)
        {
            var responseStream = GetStreamFromResource(response);

            Stream payloadStream = null;
            if (payload != null)
                payloadStream = GetStreamFromResource(payload);

            if(payloadStream != null)
                JoinStreams(responseStream, payloadStream);

            return responseStream;
        }

        public Stream GetResponseStream(NntpResponseCode responseCode, Stream payloadStream, params string[] args)
        {
            string responseStatusLine = string.Format("{0} {1}\r\n", (int)responseCode, string.Join(" ", args));

            Stream responseStream = GetStreamFromString(responseStatusLine);

            if(payloadStream != null)
                JoinStreams(responseStream, payloadStream);
            return responseStream;
        }

        public Stream GetResponseStream(NntpResponseCode responseCode, bool includeServerGreeting, bool generatePayload)
        {
            string serverGreeting = string.Empty;
            
            if(includeServerGreeting)
                serverGreeting = "200 Hello From Test Server\r\n";

            string responseStatusLine = string.Format("{0} Test Test\r\n", (int)responseCode);

            string responsePayload = string.Empty;

            if (generatePayload)
            {
                string value = "Path: news.free.fr!xref-2.proxad.net!spooler1c-1.proxad.net!cleanfeed2-a.proxad.net!proxad.net!feeder1-1.proxad.net!feeder.news-service.com!newsfeed101.telia.com!nf02.dk.telia.net!news.tele.dk!feed118.news.tele.dk!dotsrc.org!filter.dotsrc.org!news.dotsrc.org!not-for-mail\r\n";
                value += "Message-ID: <2v22g5tgqijmpnq08phvo9s69asimesi21@4ax.com>\r\n";
                value += "From: John Fitzsimons <DELETEucwubqf02@sneakemail.com>\r\n";
                value += "Newsgroups: news.software.nntp\r\n";
                value += "Subject: Unix NNTP server software for \"local\" groups ?\r\n";
                value += "Date: Mon, 16 Nov 2009 19:27:33 +1100\r\n";
                value += "Lines: 6\r\n";
                value += "Reply-To: DELETEucwubqf02@sneakemail.com\r\n";
                value += "X-Newsreader: Forte Agent 6.00/32.1183\r\n";
                value += "MIME-Version: 1.0\r\n";
                value += "Content-Type: text/plain; charset=us-ascii\r\n";
                value += "Content-Transfer-Encoding: 7bit\r\n";
                value += "Organization: SunSITE.dk - Supporting Open source\r\n";
                value += "NNTP-Posting-Host: 114.76.0.218\r\n";
                value += "X-Trace: news.sunsite.dk DXC=c_0eB8S?9\\JlZiKmBn9NRLYSB=nbEKnkK__faG2b50Q@[iP1NVFo?_DbBeM0l<XeSNWYH7`Me]\\IDhE4XHPT2[HB7nag9e?5L[C\r\n";
                value += "X-Complaints-To: staff@sunsite.dk\r\n";
                value += "Xref: datacenter.viste-family.net news.software.nntp:185\r\n";
                value += "\r\n";
                value += "\r\n";
                value += "Can anyone suggest appropriate software please ? Preferably for\r\n";
                value += "someone who is a total Unix \"newbie\".\r\n";
                value += "\r\n";
                value += "\r\n";
                value += "Regards, John.\r\n";
                value += ".\r\n";
                responsePayload = value;
            }
            
            string responseData = string.Format("{0}{1}{2}", serverGreeting, responseStatusLine, responsePayload);
            return GetStreamFromString(responseData);
        }

        public void JoinStreams(Stream destination, params Stream[] streams)
        {
            if(destination == null)
                throw new ArgumentNullException("destination");

            foreach (Stream stream in streams)
            {
                stream.CopyTo(destination);
            }
            destination.Position = 0;
        }

        public Stream GetStreamFromString(string val)
        {
            byte[] data = Encoding.UTF8.GetBytes(val);
            return new MemoryStream(data);
        }

        public Stream GetStreamFromResource(string resourceFileName)
        {
            return
                Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(string.Format("Helvetica.NntpClient.Test.Resources.{0}", resourceFileName));
        }
    }
}