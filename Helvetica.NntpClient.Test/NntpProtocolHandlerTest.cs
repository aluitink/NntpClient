using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Helvetica.NntpClient.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Helvetica.NntpClient.Test
{
    [TestClass]
    public class NntpProtocolHandlerTest: TestBase
    {
        [TestMethod]
        public void StreamInitializationShouldNotThrowException()
        {
            var stream = GetResponseStream(NntpResponseCode.ServerReadyPostingAllowed, false, false);
            NntpProtocolHandler handler = new NntpProtocolHandler(new StreamAccessor(stream));
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void InvalidResponseStreamInitializationShouldThrowException()
        {
            var stream = GetResponseStream(NntpResponseCode.Unknown, false, false);
            NntpProtocolHandler handler = new NntpProtocolHandler(new StreamAccessor(stream));
        }

        [TestMethod]
        public void CanHandleBlockResponse()
        {
            var stream = GetResponseStream(NntpResponseCode.ArticleRetrievedHeadAndBodyFollows, true, true);

            Mock<StreamAccessor> streamAccessorMock = new Mock<StreamAccessor>(stream, null);
            streamAccessorMock.SetupGet(s => s.BaseStream).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.AutoFlush).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.NewLine).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.ReadLine()).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.WriteLine(It.IsAny<string>())).Verifiable();

            NntpProtocolHandler handler = new NntpProtocolHandler(streamAccessorMock.Object);
            
            var response = handler.PerformRequest(new NntpRequest(NntpCommand.Article, false, "185"));

            Assert.AreEqual(response.ResponseCode, NntpResponseCode.ArticleRetrievedHeadAndBodyFollows);

            Assert.IsNotNull(response.StatusLine);
            Assert.IsNotNull(response.Payload);
        }

        [TestMethod]
        public void CanHandleMultipleRequests()
        {
            var stream1 = GetResponseStream(NntpResponseCode.ArticleRetrievedHeadAndBodyFollows, true, true);
            var stream2 = GetResponseStream(NntpResponseCode.ArticleRetrievedHeadAndBodyFollows, false, true);

            MemoryStream stream = new MemoryStream();

            JoinStreams(stream, stream1, stream2);

            Mock<StreamAccessor> streamAccessorMock = new Mock<StreamAccessor>(stream, null);
            streamAccessorMock.SetupGet(s => s.BaseStream).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.AutoFlush).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.NewLine).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.ReadLine()).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.WriteLine(It.IsAny<string>())).Verifiable();

            NntpProtocolHandler handler = new NntpProtocolHandler(streamAccessorMock.Object);

            var response1 = handler.PerformRequest(new NntpRequest(NntpCommand.Article, false, "185"));

            Assert.AreEqual(response1.ResponseCode, NntpResponseCode.ArticleRetrievedHeadAndBodyFollows);

            Assert.IsNotNull(response1.StatusLine);
            Assert.IsNotNull(response1.Payload);
            
            var response2 = handler.PerformRequest(new NntpRequest(NntpCommand.Article, false, "186"));

            Assert.AreEqual(response2.ResponseCode, NntpResponseCode.ArticleRetrievedHeadAndBodyFollows);
            
            Assert.IsNotNull(response2.StatusLine);
            Assert.IsNotNull(response2.Payload);
            
        }

        [TestMethod]
        public void CanHandleUnexpectedResult()
        {
            var stream1 = GetResponseStream(NntpResponseCode.ArticleRetrievedRequestTextSeparately, true, true);
            var stream2 = GetResponseStream(NntpResponseCode.ArticleRetrievedHeadAndBodyFollows, false, true);

            MemoryStream stream = new MemoryStream();

            JoinStreams(stream, stream1, stream2);

            Mock<StreamAccessor> streamAccessorMock = new Mock<StreamAccessor>(stream, null);
            streamAccessorMock.SetupGet(s => s.BaseStream).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.AutoFlush).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.NewLine).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.ReadLine()).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.WriteLine(It.IsAny<string>())).Verifiable();

            NntpProtocolHandler handler = new NntpProtocolHandler(streamAccessorMock.Object);

            var response1 = handler.PerformRequest(new NntpRequest(NntpCommand.Article, false, "185"));

            Assert.AreEqual(response1.ResponseCode, NntpResponseCode.ArticleRetrievedRequestTextSeparately);

            var response2 = handler.PerformRequest(new NntpRequest(NntpCommand.Article, false, "186"));

            Assert.AreEqual(response2.ResponseCode, NntpResponseCode.Unknown);
        }

        [TestMethod, Timeout(5000)]//Test should timeout in 5 seconds
        [ExpectedException(typeof(TimeoutException))]
        public void StreamAccessorTimeoutsAreUnhandled()
        {
            var stream = GetResponseStream(NntpResponseCode.ArticleRetrievedHeadAndBodyFollows, true, true);

            Mock<StreamAccessor> streamAccessorMock = new Mock<StreamAccessor>(stream, null);
            streamAccessorMock.SetupGet(s => s.BaseStream).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.AutoFlush).CallBase().Verifiable();
            streamAccessorMock.Setup(s => s.NewLine).CallBase().Verifiable();

            int callCount = 0;
            streamAccessorMock.Setup(s => s.ReadLine()).Callback(() =>
            {
                if(callCount >= 2)//Timeout on body read
                    Thread.Sleep(3000);
                callCount++;
            }).CallBase();
            streamAccessorMock.Setup(s => s.WriteLine(It.IsAny<string>())).Verifiable();

            NntpProtocolHandler handler = new NntpProtocolHandler(streamAccessorMock.Object, 2000); //set timeout to 2 seconds

            //We need the timeout exception to buble up to the caller so they are aware they need to retry
            var response = handler.PerformRequest(new NntpRequest(NntpCommand.Article, false, "123")); // No data available, this should timeout
        }

        [TestMethod]
        [Ignore]
        public void StreamReadShouldTimeoutCanRecover()
        {
            var stream1 = GetResponseStream(NntpResponseCode.ServerReadyPostingAllowed, false, false);

            string serverHello = "200 Test NntpStatus\r\n";

            string status = "220 185 <2v22g5tgqijmpnq08phvo9s69asimesi21@4ax.com> article retrieved - text follows\r\n";
            string line1 = "Path: news.free.fr!xref-2.proxad.net!spooler1c-1.proxad.net!cleanfeed2-a.proxad.net!proxad.net!feeder1-1.proxad.net!feeder.news-service.com!newsfeed101.telia.com!nf02.dk.telia.net!news.tele.dk!feed118.news.tele.dk!dotsrc.org!filter.dotsrc.org!news.dotsrc.org!not-for-mail\r\n";
            string line2 = "Message-ID: <2v22g5tgqijmpnq08phvo9s69asimesi21@4ax.com>\r\n";
            string line3 = "From: John Fitzsimons <DELETEucwubqf02@sneakemail.com>\r\n";
            string line4 = ".\r\n";

            var payload = serverHello + status + line1 + line2 + line3 + line4;
            var stream = GetStreamFromString(payload);

            Mock<IStreamAccessor> streamAccessorMock = new Mock<IStreamAccessor>();
            streamAccessorMock.SetupGet(s => s.BaseStream).Returns(stream);
            streamAccessorMock.Setup(s => s.WriteLine(It.IsAny<string>())).Verifiable();
            streamAccessorMock.SetupSequence(s => s.ReadLine())
                .Returns(serverHello)
                .Throws(new TimeoutException())
                .Returns(status)
                .Returns(line1)
                .Returns(line2)
                .Returns(line3)
                .Returns(line4);

            NntpProtocolHandler handler = new NntpProtocolHandler(streamAccessorMock.Object);

            NntpResponse response;
            try
            {
                //response = handler.PerformRequest(request); // No data available, this should timeout
            }
            catch (TimeoutException e)
            {
                //Retry
                //response = handler.PerformRequest(request); // Data is again available
            }

            //Assert.IsNotNull(response);
            //Assert.AreEqual(NntpResponseCode.ArticleRetrievedHeadAndBodyFollows, response.ResponseCode);
        }
    }
}
