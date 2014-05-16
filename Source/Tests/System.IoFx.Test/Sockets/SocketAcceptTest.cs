﻿using System.IoFx.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IoFx.Test.Sockets
{
    [TestClass]
    public class SocketAcceptTest
    {
        [TestMethod]
        public void SocketConnect()
        {
            const int port = 5050;
            const int clientCount = 10;
            var sockets = SocketEvents.GetTcpStreamSockets(port);
            try
            {
                var tcs = new TaskCompletionSource<object>();
                int count = 0;
                sockets.Subscribe(s =>
                {
                    count++;
                    if (count == clientCount)
                    {
                        tcs.TrySetResult(null);
                    }
                    s.Close();
                },
                tcs.SetException,
                () => tcs.TrySetResult(null));

                for (int i = 0; i < clientCount; i++)
                {
                    SocketTestUtility.Connect(port);
                }

                tcs.Task.Wait(Defaults.MediumTestWaitTime);
                Assert.IsTrue(count == clientCount);
            }
            finally
            {
                sockets.Dispose();
            }
        }


    }
}
