using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Sockets;
using Helvetica.NntpClient.CommandQuery;
using Helvetica.NntpClient.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace Helvetica.NntpClient.Test
{
    [TestClass]
    public class NntpClientTest: TestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var serverGreeting = GetResponseResources("ServerGreeting.txt");
            //var authInfoUser = GetResponseResources("AuthInfoUserResponse.txt");
            //var authInfoPass = GetResponseResources("AuthInfoPassResponse.txt");
            //var list = GetResponseResources("ListResponse.txt", "ListPayload.txt");
            //var setGroup = GetResponseResources("GroupResponse.txt");

            //MemoryStream ms = new MemoryStream();

            //JoinStreams(ms, serverGreeting, authInfoUser, authInfoPass, list, setGroup);

            //Mock<StreamAccessor> streamAccessorMock = new Mock<StreamAccessor>(ms, null);
            //streamAccessorMock.SetupGet(s => s.BaseStream).CallBase().Verifiable();
            //streamAccessorMock.Setup(s => s.ReadLine()).CallBase().Verifiable();
            //streamAccessorMock.Setup(s => s.WriteLine(It.IsAny<string>())).Verifiable();
            
            //Mock<NntpClient> nntpClientMock = new Mock<NntpClient>("localhost", 123, false, null);
            //nntpClientMock.Protected()
            //    .SetupGet<NntpProtocolHandler>("ProtocolHandler")
            //    .Returns(new NntpProtocolHandler(streamAccessorMock.Object));
            //nntpClientMock.Setup(c => c.Connect()).Verifiable();
            //nntpClientMock.Setup(c => c.Disconnect()).Verifiable();

            //var nntpClient = nntpClientMock.Object;

            //using (nntpClient)
            //{
            //    nntpClient.Connect();

            //    var authResult = nntpClient.Execute(new Authenticate("test", "test"));
            //    Assert.IsTrue(authResult);
            //    Assert.IsTrue(nntpClient.Authenticated);

            //    nntpClient.Group("alt.test");

            //    nntpClient.Disconnect();
            //}
        }


        [TestMethod]
        public void TestMethod2()
        {
        //    NntpClient client = new NntpClient("address", 119, false);

        //    client.Connect();
        //    var auth = client.Authenticate("username", "Password");
        //    Assert.IsTrue(auth);
        //    var group = client.Group("misc.test");
        //    Assert.IsTrue(group);
        //    Assert.IsNotNull(client.CurrentGroup);
        //    var first = client.CurrentGroup.First;
        //    var last = client.CurrentGroup.Last;
        //    var stat = client.Stat(first);
        //    Assert.IsTrue(stat);
        //    Assert.AreEqual(first, client.CurrentArticleId);
        //    var next = client.Next();
        //    Assert.IsTrue(next);
        //    Assert.AreEqual(first + 1, client.CurrentArticleId);

        //    var headers = client.Headers("Subject", first, last);

        //    foreach (KeyValuePair<long, string> keyValuePair in headers.Articles)
        //    {
        //        long articleId = keyValuePair.Key;
        //        string value = keyValuePair.Value;
        //    }
        }
    }
}
